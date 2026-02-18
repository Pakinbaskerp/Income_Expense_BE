using FinanceService.Application.Contract.Services;
using FluentValidation;
using MediatR;
using FinanceService.Domain.Dto;

namespace FinanceService.Application.CQRS.Command;

public record LoginCommand(string Username, string Password) : IRequest<LoginResponseDto>;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("UserName Required");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is not Empty");
    }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IAuthService _authService;
    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Guid? isUserExist = await _authService.IsUserExist(request.Username, cancellationToken);

        if (isUserExist == null)
        {
            throw new Exception("User Not Found");
        }

        bool isPasswordValid =  _authService.IsPasswordValid(isUserExist, request.Password);

        if(!isPasswordValid)
        {
            throw new Exception("Invalid Password");
        }

        string? accessToken = _authService.GenerateAccessToken(isUserExist.Value,request.Username);

        LoginResponseDto loginResponse = new()
        {
          Token = accessToken  
        };
        return loginResponse;
    }
}
