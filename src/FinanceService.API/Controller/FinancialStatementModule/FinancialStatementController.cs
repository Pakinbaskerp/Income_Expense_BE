using FinanceService.Application.CQRS.FinanceStatement.Command;
using FinanceService.Application.CQRS.FinanceStatementModule.Command;
using FinanceService.Application.CQRS.FinanceStatementModule.Query;
using FinanceService.Domain.Dto;
using FinanceService.Infrastructure.Contract.Service;
using libraries.logging.Contract;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinanceService.API.Controller.FinancialStatementModule;

[ApiController]
[Route("api/v1/finance")]
public class FinancialStatementController : ControllerBase
{
    private readonly ILoggerManager<FinancialStatementController> _logger;
    private readonly IMediator _mediator;
    public FinancialStatementController(
        ILoggerManager<FinancialStatementController> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("statement/list")]
    [ProducesResponseType(typeof(List<FinanceAccountResponseDto>), StatusCodes.Status200OK)]

    public async Task<IActionResult> GetFinancialStatements(
        [FromQuery(Name = "tax-year-id")] Guid taxYearId,
        [FromQuery(Name = "user-id")] Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogInfo("Fetching financial statements");
        GetFinanceStatementAccountQuery query = new(userId, taxYearId, cancellationToken);
        List<FinanceAccountResponseDto> result = await _mediator.Send(query);
        _logger.LogInfo("Fetched fiancial statement accounts");
        return result.Count() <= 0 ? NoContent() : Ok(result);
    }

    [HttpPost("statement/create")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateFianancialStatement(
        [FromQuery(Name = "tax-year-id")] Guid taxYearId,
        [FromQuery(Name = "user-id")] Guid userId,
        [FromBody] FinanceAccountRequestDto financeAccountRequestDto,
        CancellationToken cancellationToken = default
    )
    {
        CreateFinanceStatementCommand command = new(
            userId,
            financeAccountRequestDto.AccountName!,
            financeAccountRequestDto.StartDate,
            financeAccountRequestDto.EndDate,
            financeAccountRequestDto.CurrencyCode,
            taxYearId,
            cancellationToken);
        Guid result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("statement/update")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateFinancialStatement(
        [FromQuery(Name = "account-id")] Guid accountId,
        [FromBody] FinanceAccountRequestDto financeAccountRequestDto,
        CancellationToken cancellationToken
    )
    {
        UpdateFinanceStatementCommand command = new(accountId, financeAccountRequestDto.AccountName, financeAccountRequestDto.StartDate, financeAccountRequestDto.EndDate, financeAccountRequestDto.CurrencyCode, cancellationToken);
        Guid result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("statement/delete")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteFinancialStatement(
        [FromQuery(Name = "account-id")] Guid accountId,
        CancellationToken cancellationToken = default
    )
    {
        DeleteFinanceStatementCommand command = new(accountId, cancellationToken);
        Guid result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("account/create")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateFinanceAccountDetail(
        [FromQuery(Name = "fin-statement-id")] Guid finStatementId,
        [FromBody] List<FinanceAccountInputDto> financeAccountInputDtoList,
        CancellationToken cancellationToken = default
    )
    {
        CreateFinancialAccountListCommand command = new(financeAccountInputDtoList, finStatementId, cancellationToken);
        Guid result = await _mediator.Send(command);
        return Ok(result);
    }


    [HttpDelete("account/delete")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]

    public async Task<IActionResult> DeleteFinancialAccountDetail(
        [FromQuery(Name = "fin-statement-id")] Guid finStatementId,
        CancellationToken cancellationToken = default
    )
    {
        return Ok();
    }

    [HttpPut("account/update")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateFinancialAccountDetail(
        [FromQuery(Name = "fin-account-id")] Guid finAccountId,
        [FromBody] FinanceAccountInputDto financeAccountInputDto,
        CancellationToken cancellationToken = default
    )
    {
        return Ok();
    }

    [HttpGet("statement/{statement-id}/get")]
    [ProducesResponseType(typeof(FinanceStateDisplayDetailDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFinancialStatementList(
        [FromRoute(Name = "statement-id")] Guid statementId,
        [FromQuery(Name = "search-key")] string? SearchKey,
        [FromQuery(Name = "row-index")] int rowIndex = 0,
        [FromQuery(Name = "page-size")] int pageSize = 20,
        [FromQuery(Name = "is-ascending")] bool isAscending = true,
        CancellationToken cancellationToken = default
    )
    {
        GetFinancialAccountListQuery query = new(statementId, cancellationToken);
        FinanceStateDisplayDetailDto result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpPut("statement/upsert")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpsertFinancialStatement(
        [FromBody] FinancialStatementUpsertDto financialStatementUpsertDto,
        CancellationToken cancellationToken = default
    )
    {
        UpsertFinancialStatementCommand command = new UpsertFinancialStatementCommand(
            financialStatementUpsertDto.financialAccountOutputDtos,
            financialStatementUpsertDto.FinanceStatementId
        );

        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

}