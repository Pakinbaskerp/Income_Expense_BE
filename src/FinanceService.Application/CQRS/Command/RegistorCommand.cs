using FinanceService.Application.Contract.Services;
using FluentValidation;
using MediatR;

namespace FinanceService.Application.CQRS.Command;

public record RegisterCommand(string FirstName, string LastName, string Email, string Password, string ConfirmPassword, CancellationToken CancellationToken) : IRequest<Guid>;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Passwords do not match");
    }
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Guid>
{
    private readonly IAuthService _authService;
    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (request.Password != request.ConfirmPassword)
        {
            throw new ArgumentException("Passwords do not match");
        }

        Guid? userId = await _authService.CreateUserAsync(request.FirstName, request.LastName, request.Email, request.CancellationToken);

        await _authService.SetUserPassword( request.Password, userId.Value, request.CancellationToken);

        return userId.Value;
    }
}