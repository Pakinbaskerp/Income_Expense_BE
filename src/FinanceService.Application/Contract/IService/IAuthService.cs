using FinanceService.Domain.Dto;
using FinanceService.Domain.Models;

namespace FinanceService.Application.Contract.Services;

public interface IAuthService
{
    /// <summary>
    /// Registers a new user and returns authentication response.
    /// </summary>
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken ct);

    /// <summary>
    /// Logs in a user by validating email and password.
    /// Returns AuthResponseDto if successful, otherwise null.
    /// </summary>
    Task<AuthResponseDto?> LoginAsync(string email, string password, CancellationToken ct);

    /// <summary>
    /// Fetches a single active user by email using LINQ/EF querying.
    /// </summary>
    Task<User?> GetUserByEmailAsync(string email, CancellationToken ct);

    /// <summary>
    /// Fetches a single user using raw SQL mapped to the User model.
    /// </summary>
    Task<User?> GetUserByEmailRawAsync(string email, CancellationToken ct);

    /// <summary>
    /// Fetches all active users.
    /// </summary>
    Task<List<User>> GetAllUserAsync(CancellationToken ct);

    /// <summary>
    /// Is User Exist
    /// </summary>
    /// <param name="UserName"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<Guid?> IsUserExist(string UserName, CancellationToken ct);

    Task<Guid?> CreateUserAsync(string firstName, string lastName, string email, CancellationToken ct);

    Task SetUserPassword(string password, Guid userId, CancellationToken ct);
    bool IsPasswordValid(Guid? isUserExist, string password);
    string GenerateAccessToken(Guid userId, string email);
}
