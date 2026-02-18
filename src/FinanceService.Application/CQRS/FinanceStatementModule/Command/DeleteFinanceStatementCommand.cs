using FinanceService.Application.Contract.IService;
using FluentValidation;
using libraries.logging.Contract;
using MediatR;

namespace FinanceService.Infrastructure.Contract.Service;

public record DeleteFinanceStatementCommand(
    Guid FinancialStatementId,
    CancellationToken CancellationToken
) : IRequest<Guid>;

public class DeleteFinanceStatementCommandValidator : AbstractValidator<DeleteFinanceStatementCommand>
{
    public DeleteFinanceStatementCommandValidator()
    {
        RuleFor(x => x.FinancialStatementId).NotEmpty().WithMessage("FinancialStatementId is required.");
    }
}
public class DeleteFinanceStatementCommandHandler : IRequestHandler<DeleteFinanceStatementCommand, Guid>
{
    private readonly IFinanceStatementService _financeStatementService;
    private readonly ILoggerManager<DeleteFinanceStatementCommandHandler> _logger;

    public DeleteFinanceStatementCommandHandler(
        IFinanceStatementService financeStatementService,
        ILoggerManager<DeleteFinanceStatementCommandHandler> logger
    )
    {
        _financeStatementService = financeStatementService;
        _logger = logger;
    }

    public async Task<Guid> Handle(DeleteFinanceStatementCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"Handling DeleteFinanceStatementCommand for FinancialStatementId: {request.FinancialStatementId}");

        await _financeStatementService.DeleteFinancialStatementAsync(request.FinancialStatementId, request.CancellationToken);

        _logger.LogInfo($"Successfully deleted FinancialStatementId: {request.FinancialStatementId}");

        return request.FinancialStatementId;
    }
}