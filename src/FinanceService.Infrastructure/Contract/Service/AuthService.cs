using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FinanceService.Application.Contract.IRepository;
using FinanceService.Application.Contract.Services;
using FinanceService.Domain.Dto;
using FinanceService.Domain.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FinanceService.Application.Services;

public sealed class AuthService : IAuthService
{
    private readonly IRepositoryBase _repo;
    private readonly PasswordHashingOptions _options;
    private readonly ILogger<AuthService> _logger;
    private readonly HashAlgorithmName _algo;
    private readonly IConfiguration _config;

    public AuthService(IRepositoryBase repo,
        ILogger<AuthService> logger,
        IOptions<PasswordHashingOptions> options,
        IConfiguration config
        )
    {
        _repo = repo;
        _logger = logger;
        _options = options.Value;
        _algo = new HashAlgorithmName(_options.Algorithm);
        _config = config;
    }

    // ------------------- REGISTER -------------------
    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken ct)
    {
        // Check if user already exists
        var existingUser = await _repo.FirstOrDefaultAsync<User>(
            u => u.Email == request.Email, ct, true);

        if (existingUser is not null)
            throw new InvalidOperationException("User already exists with this email.");

        // Create new user
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PictureUrl = request.PictureUrl,
            IsActive = true,
        };

        // Transaction block
        var tx = await _repo.BeginTransactionAsync(ct);
        try
        {
            await _repo.AddAsync(user, ct);
            await _repo.SaveChangesAsync(ct);

            await _repo.CommitTransactionAsync(tx, ct);
        }
        catch
        {
            await _repo.RollbackTransactionAsync(tx, ct);
            throw;
        }

        _logger.LogInformation("New user registered: {UserId}", user.Id);

        return new AuthResponseDto
        {
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PictureUrl = user.PictureUrl,
            Token = "dummy-jwt-token"
        };
    }

    // ------------------- LOGIN -------------------
    public async Task<AuthResponseDto?> LoginAsync(string email, string password, CancellationToken ct)
    {
        var user = await _repo.FirstOrDefaultAsync<User>(
            u => u.Email == email && u.IsActive, ct, true);

        if (user is null)
            return null;


        return new AuthResponseDto
        {
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PictureUrl = user.PictureUrl,
            Token = "dummy-jwt-token"
        };
    }

    // ------------------- GET USER BY EMAIL -------------------
    public Task<User?> GetUserByEmailAsync(string email, CancellationToken ct)
        => _repo.FirstOrDefaultAsync<User>(u => u.Email == email, ct, true);

    /// <summary>
    /// Raw SQL version: fetch a user by email and map directly into the User model.
    /// Works with EF Core SqlQueryRaw<T>. Keep columns/aliases matching property names.
    /// </summary>
    public async Task<User?> GetUserByEmailRawAsync(string email, CancellationToken ct)
    {


        // If your table/columns already match the model property names (PascalCase):
        const string sql = """
            SELECT 
                Id,
                FirstName,
                LastName,
                Email,
                PictureUrl,
                IsActive
            FROM Users
            WHERE Email = $email
            LIMIT 1;
        """;

        DbParameter[] p = [new SqliteParameter("$email", email)];

        var results = await _repo.QuerySqlAsync<User>(sql, p, ct);
        var user = results.FirstOrDefault();

        if (user is null)
        {
            _logger.LogInformation("No user found for email {Email}", email);
            return null;
        }

        _logger.LogDebug("Fetched user {UserId} via raw SQL", user.Id);
        return user;
    }

    public async Task<List<User>> GetAllUserAsync(CancellationToken ct)
    {
        var users = _repo.FindByCondition<User>(x => x.IsActive == true);
        return await users.ToListAsync(ct);
    }

    /// <inheritdoc/>
    public async Task<Guid?> IsUserExist(string userName, CancellationToken ct)
    {
        User? user =  _repo.FindByCondition<User>(x=> x.IsActive && x.Email == userName).FirstOrDefault();

        if (user is null)
        {
            Exception exception = new Exception("User Not Found");
            throw exception;
        }

        return user.Id;
    }

    public async Task<Guid?> CreateUserAsync(string firstName, string lastName, string email, CancellationToken ct)
    {
        User user = new()
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email
        };

        await _repo.AddAsync<User>(user, ct);
        await _repo.SaveChangesAsync(ct);
        return user.Id;
    }

    public async Task SetUserPassword(string password, Guid userId, CancellationToken ct)
    {
        (string saltValue, string hashValue) = HashPassword(password);
        AuthDetail authDetail = new()
        {
            UserId = userId,
            PasswordSalt = saltValue,
            PasswordHash = hashValue,
            PasswordChangedAt = DateTime.UtcNow
        };

        await _repo.AddAsync<AuthDetail>(authDetail, ct);
        await _repo.SaveChangesAsync(ct);
    }

    private (string salt, string passwordHash) HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(_options.SaltSize);
        string peppered = password + _options.PepperKey;

        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(peppered),
            salt,
            _options.Iterations,
            _algo,
            _options.KeySize);

        return (Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    public bool IsPasswordValid(Guid? userId, string password)
    {
        AuthDetail? authDetail = _repo.FindByCondition<AuthDetail>(x => x.IsActive && x.UserId == userId).FirstOrDefault();

        return VerifyPassword(authDetail!.PasswordSalt!, authDetail!.PasswordHash!, password);

    }

    private bool VerifyPassword(string passwordSalt, string passwordHash, string password)
    {
        string peppered = password + _options.PepperKey;

        byte[] salt = Convert.FromBase64String(passwordSalt);
        byte[] storedHash = Convert.FromBase64String(passwordHash);

        byte[] computedHash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(peppered),
            salt,
            _options.Iterations,
            _algo,
            _options.KeySize);

        return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
    }

    public string GenerateAccessToken(Guid userId, string email)
    {
        var key = _config["Jwt:Key"];
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];
        var expires = int.Parse(_config["Jwt:ExpiresMinutes"] ?? "30");

        if(key is null)
        {
            throw new Exception("Key is null");
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim("UserId", userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expires),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
