using System.Runtime.Serialization;

namespace FinanceService.Domain.Dto;

public class LoginDto
{
    public string Username { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
}


public sealed class RegisterRequestDto
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PictureUrl { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}

public sealed class AuthResponseDto
{
    public Guid UserId { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PictureUrl { get; init; } = string.Empty;

    public string Token { get; init; } = string.Empty;
}
