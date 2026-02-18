using FinanceService.Domain.Dto;
using FinanceService.Domain.Models;

namespace FinanceService.Application.Contract.IService;

public interface IFinanceStatementService
{
    Task<IEnumerable<FinanceAccountResponseDto>> GetFinancialStatementsAsync(Guid userId, Guid taxYearId,CancellationToken cancellationToken);
    Task CreateFinancialStatementAsync(UserFinancialStatementMapping userFinancialStatementMapping, FinancialStatement financialStatement, CancellationToken cancellationToken);
    Task DeleteFinancialStatementAsync(Guid financialStatementId, CancellationToken cancellationToken);
    Task UpdateFinancialStatementAsync(Guid financialStatementId, string? financeStatementName, DateTime? startDate, DateTime? endDate, Guid? currencyCode, CancellationToken cancellationToken);
    Task CreateFinanceAccountAsync(List<FinancialAccount> financialAccountList, List<FinancialStatementAccountMapping> financialStatementAccountMappingList, CancellationToken cancellationToken);
    Task<Guid> UpdateFinanceAccountAsync(Guid financialAccountId, DateTime? creditDate, string? accountName, Guid? bankAccountId, decimal? amount, CancellationToken cancellationToken);
    Task<Guid> DeleteFinancialAccountAsync(Guid financialAccountId, CancellationToken cancellationToken);
    Task<IQueryable<FinancialAccountOutputDto>> GetFinancialAccountAsync(Guid financialStatementId, CancellationToken cancellationToken);
    Task UpsertFinancialStatementAsync(List<FinancialAccountOutputDto> financialAccountOutputDtos, Guid financeStatementId, CancellationToken cancellationToken);
    Task<List<GuidIdNameDto>> FetchBankAndAccountDetails(Guid guid, CancellationToken cancellationToken);
}