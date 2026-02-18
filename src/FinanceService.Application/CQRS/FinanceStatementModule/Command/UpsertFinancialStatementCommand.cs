

using FinanceService.Application.Contract.IService;
using FinanceService.Domain.Dto;
using MediatR;

namespace FinanceService.Application.CQRS.FinanceStatementModule.Command;

public record UpsertFinancialStatementCommand(
   List<FinancialAccountOutputDto> financialAccountOutputDtos,
   Guid FinanceStatementId
) : IRequest<Unit>;

public record UpsertFinancialStatementCommandHandler : IRequestHandler<UpsertFinancialStatementCommand, Unit>
{
    private readonly IFinanceStatementService _financeStatementService;

    public UpsertFinancialStatementCommandHandler(IFinanceStatementService financeStatementService)
    {
        _financeStatementService = financeStatementService;
    }

    public async Task<Unit> Handle(UpsertFinancialStatementCommand request, CancellationToken cancellationToken)
    {
        await _financeStatementService.UpsertFinancialStatementAsync(request.financialAccountOutputDtos, request.FinanceStatementId, cancellationToken);
        return Unit.Value;
    }
}