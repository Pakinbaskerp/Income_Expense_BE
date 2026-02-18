using FinanceService.Application.CQRS.AccountModule;
using FinanceService.Application.CQRS.AccountModule.Command;
using FinanceService.Domain.Dto;
using FinanceService.Infrastructure.Contract.Service;
using libraries.logging.Contract;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinanceService.API.Controller.AccountModule;

[ApiController]
[Route("api/v1/account")]
public class AccountController : ControllerBase
{
    private readonly ILoggerManager<AccountController> _logger;
    private readonly IMediator _mediator;

    public AccountController(
        ILoggerManager<AccountController> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("user/{user-id}/list")]
    [ProducesResponseType(typeof(List<AccountResponseDto>), StatusCodes.Status200OK)]

    public async Task<IActionResult> GetAccountListForUser(
        [FromRoute(Name = "user-id")]  Guid userId
    )
    {
        _logger.LogInfo($"Received request to get account list for UserId: {userId}");
        GetAccountListForUserQuery query = new (userId);
        List<AccountResponseDto>? result = await _mediator.Send(query);
        _logger.LogInfo($"Fetched account list for UserId: {userId} with {result.Count} accounts.");
        return result.Count() > 0 ? Ok(result) : NoContent();
    }

    [HttpPost("user/{user-id}/create-account-detail")]
    public async Task<IActionResult> CreateAccountDetail(
        [FromRoute(Name = "user-id")] Guid userId, 
        [FromBody] AccountDetailDto accountDetailDto,
        CancellationToken cancellationToken = default
        )
    {
        _logger.LogInfo($"Received request to create account detail for BankName: {accountDetailDto.BankName}");
        CreateAccountDetailCommand command = new (
            UserId: userId, 
            AccountName: accountDetailDto.BankName,
            AccountType: "DefaultType",
            InitialBalance: accountDetailDto.Balance,
            IsCalculated: accountDetailDto.IsCountable,
            CancellationToken: cancellationToken
        );
        Guid accountId = await _mediator.Send(command);
        _logger.LogInfo($"Created account detail with Id: {accountId} for BankName: {accountDetailDto.BankName}");
        return CreatedAtAction(null, accountId);
    }

    [HttpPatch("update-account-detail")]
    public async Task<IActionResult> UpdateAccountDetail(
        [FromBody] UpdateAccountDetailDto updateAccountDetailDto,
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogInfo($"Received request to update account detail for BankAccountDetailId: {updateAccountDetailDto.BankAccountDetailId}");
        UpdateAccountDetailCommand command = new (
            BankAccountDetailId: updateAccountDetailDto.BankAccountDetailId,
            AccountName: updateAccountDetailDto.AccountName,
            Balance: updateAccountDetailDto.Balance,
            CurrencyCode: updateAccountDetailDto.CurrencyCode,
            IsCalculate: updateAccountDetailDto.IsCalculate,
            CancellationToken: cancellationToken
        );
        Guid updatedAccountId = await _mediator.Send(command);
        _logger.LogInfo($"Updated account detail for BankAccountDetailId: {updatedAccountId}");
        return Ok(updatedAccountId);
    }

    [HttpDelete("delete-account-detail/{bank-account-detail-id}")]
    public async Task<IActionResult> DeleteAccountDetail(
        [FromRoute(Name = "bank-account-detail-id")] Guid bankAccountDetailId,
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogInfo($"Received request to delete account detail for BankAccountDetailId: {bankAccountDetailId}");
        DeleteAccountDetailCommand command = new (bankAccountDetailId, cancellationToken);
        await _mediator.Send(command);
        _logger.LogInfo($"Deleted account detail for BankAccountDetailId: {bankAccountDetailId}");
        return NoContent();
    }
}