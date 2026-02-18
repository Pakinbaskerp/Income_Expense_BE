using FinanceService.Application.Contract.IService;
using FinanceService.Domain.Dto;
using FinanceService.Domain.Models;
using libraries.logging.Contract;
using MediatR;

namespace FinanceService.Application.CQRS.FinanceStatement.Command;

public sealed record CreateFinancialAccountListCommand(
    List<FinanceAccountInputDto> FinanceAccountInputDto,
    Guid FinancialStatementId,
    CancellationToken CancellationToken
): IRequest<Guid>;

public class CreateFinancialAccountListCommandHandler(
    IFinanceStatementService financeStatementService,
    ILoggerManager<CreateFinancialAccountListCommandHandler> logger
)
 : IRequestHandler<CreateFinancialAccountListCommand, Guid>
{
    private readonly ILoggerManager<CreateFinancialAccountListCommandHandler> _logger = logger;
    private readonly IFinanceStatementService _financialStatementService = financeStatementService;
    public async Task<Guid> Handle(CreateFinancialAccountListCommand request, CancellationToken cancellationToken)
    {
        List<FinancialAccount>? financialAccountList= request.FinanceAccountInputDto
            .Select( dto =>
                new FinancialAccount
                {
                    Id = Guid.NewGuid(),
                    AccountName = dto.AccountName!,
                    AccountType = dto.AccountType,
                    BankAccountId = dto.BankAccountId,
                    CreditDate = dto.CreditDate,
                    Amount = dto.Amount
                }
            ).ToList();

        List<FinancialStatementAccountMapping>? financialStatementList = financialAccountList
            .Select(dto => 
                new FinancialStatementAccountMapping
                {
                   FinancialStatementId = request.FinancialStatementId,
                   FinancialAccountId = dto.Id 
                }
            ).ToList();

        await _financialStatementService.CreateFinanceAccountAsync(financialAccountList, financialStatementList, cancellationToken);

        return request.FinancialStatementId;
    }
}