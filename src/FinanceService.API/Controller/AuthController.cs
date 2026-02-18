
using FinanceService.Application.CQRS.Command;
using FinanceService.Domain.Dto;
using libraries.logging.Contract;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace FinanceService.API.Controller;


[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILoggerManager<AuthController> _logger;
    public AuthController(IMediator mediator,
        ILoggerManager<AuthController> logger    
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        _logger.LogInfo("Implementing login {UserName} and {Password}", loginDto.Username, loginDto.Password);
        LoginCommand loginCommand = new(loginDto.Username, loginDto.Password);
        LoginResponseDto? loginResponse = await _mediator.Send(loginCommand);
        return Ok(loginResponse);
    }

    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> Register(
        [FromBody] RegisterDto registerDto,
        CancellationToken ct = default)
    {
        RegisterCommand command = new(registerDto.FirstName!, registerDto.LastName!, registerDto.Email!, registerDto.Password!, registerDto.ConfirmPassword!, ct);
        Guid? userId = await _mediator.Send(command);
        return Ok(userId.Value);
    }
    
}