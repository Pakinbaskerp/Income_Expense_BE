using FinanceService.Application.Contract.IService;
using libraries.logging.Contract;
using MediatR;

namespace FinanceService.Infrastructure.Contract.Service;

public record UpdateFinanceStatementCommand(
    Guid FinancialStatementId,
    string? Name,
    DateTime? StartDate,
    DateTime? EndDate,
    Guid? CurrencyCode,
    CancellationToken CancellationToken
) : IRequest<Guid>;

public class UpdateFinanceStatementCommandHandler : IRequestHandler<UpdateFinanceStatementCommand, Guid>
{
    private readonly IFinanceStatementService _financeStatementService;
    private readonly ILoggerManager<UpdateFinanceStatementCommandHandler> _logger;

    public UpdateFinanceStatementCommandHandler(
        IFinanceStatementService financeStatementService,
        ILoggerManager<UpdateFinanceStatementCommandHandler> logger
    )
    {
        _financeStatementService = financeStatementService;
        _logger = logger;
    }

    public async Task<Guid> Handle(UpdateFinanceStatementCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"Handling UpdateFinanceStatementCommand for FinancialStatementId: {request.FinancialStatementId}");

        await _financeStatementService.UpdateFinancialStatementAsync(request.FinancialStatementId, request.Name, request.StartDate, request.EndDate, request.CurrencyCode, request.CancellationToken);

        _logger.LogInfo($"Successfully updated FinancialStatementId: {request.FinancialStatementId}");

        return request.FinancialStatementId;
    }
}