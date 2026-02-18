using FinanceService.Application.CQRS.TaxYearModule;
using FinanceService.Application.CQRS.TaxYearModule.Query;
using FinanceService.Domain.Dto;
using libraries.logging.Contract;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinanceService.API.Controller.TaxYearModule;

[ApiController]
[Route("api/v1/tax-year")]
public class TaxYearController : ControllerBase
{
    private readonly ILoggerManager<TaxYearController> _logger;
    private readonly IMediator _mediator;

    public TaxYearController(
        ILoggerManager<TaxYearController> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateTaxYear(
        [FromQuery(Name = "tax-year")] string taxYear,
        CancellationToken cancellationToken = default
    )
    {
        CreateTaxYearCommand command = new(taxYear, cancellationToken);
        Guid result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("list")]
    [ProducesResponseType(typeof(List<GuidIdNameDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<GuidIdNameDto>>> GetTaxYearList(
        CancellationToken cancellationToken = default
    )
    {
        GetTaxYearListQuery query = new(cancellationToken);
        List<GuidIdNameDto> result = await _mediator.Send(query);
        return Ok(result);
    }
}
