using System.Threading.Tasks;
using FinanceService.Application.Contract.IRepository;
using FinanceService.Application.Contract.IService;
using FinanceService.Domain.Dto;
using FinanceService.Domain.Models;
using libraries.logging.Contract;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Infrastructure.Contract.Service;

public class FinanceStatementService(
    ILoggerManager<FinanceStatementService> logger,
    IRepositoryBase repositoryBase
) : IFinanceStatementService
{
    private readonly IRepositoryBase _repositoryBase = repositoryBase;
    public Task<IEnumerable<FinanceAccountResponseDto>> GetFinancialStatementsAsync(Guid userId, Guid taxYearId, CancellationToken cancellationToken)
    {
        IQueryable<FinanceAccountResponseDto>? financialAccounts = (from ufsm in _repositoryBase.FindByCondition<UserFinancialStatementMapping>(
                                                                        x => x.IsActive && x.UserId == userId, true)

                                                                    join fa in _repositoryBase.FindByCondition<FinancialStatement>(
                                                                            x => x.IsActive && x.TaxYearId == taxYearId, true)
                                                                    on ufsm.FinancialStatementId equals fa.Id

                                                                    select new FinanceAccountResponseDto
                                                                    {
                                                                        AccountId = fa.Id,
                                                                        AccountName = fa.Name,
                                                                        StartDate = fa.StartDate,
                                                                        EndDate = fa.EndDate
                                                                    }
                                                                    );
        return Task.FromResult(financialAccounts.AsEnumerable());
    }

    public async Task CreateFinancialStatementAsync(UserFinancialStatementMapping userFinancialStatementMapping, FinancialStatement financialStatement, CancellationToken cancellationToken)
    {
        await _repositoryBase.AddAsync<FinancialStatement>(financialStatement, cancellationToken);
        await _repositoryBase.SaveChangesAsync(cancellationToken);
        await _repositoryBase.AddAsync<UserFinancialStatementMapping>(userFinancialStatementMapping, cancellationToken);
        await _repositoryBase.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteFinancialStatementAsync(Guid financialStatementId, CancellationToken cancellationToken)
    {

        IQueryable<FinancialStatement>? financialStatement = _repositoryBase.Query<FinancialStatement>(false);

        _ = financialStatement.Where(x => x.IsActive && x.Id == financialStatementId).ExecuteUpdateAsync(
                                        s => s.SetProperty(b => b.IsActive, false),
                                        cancellationToken
                                    );
        IQueryable<UserFinancialStatementMapping>? userFinancialStatementMapping = _repositoryBase.Query<UserFinancialStatementMapping>(false);

        _ = userFinancialStatementMapping.Where(x => x.IsActive && x.FinancialStatementId == financialStatementId).ExecuteUpdateAsync(
                                        s => s.SetProperty(b => b.IsActive, false),
                                        cancellationToken
                                    );
    }

    public async Task UpdateFinancialStatementAsync(Guid financialStatementId, string? fiananceStatementName, DateTime? startDate, DateTime? endDate, Guid? currencyCode, CancellationToken cancellationToken)
    {

        IQueryable<FinancialStatement>? existingFinancialStatement = _repositoryBase.Query<FinancialStatement>(true);

        _ = existingFinancialStatement.Where(x => x.IsActive && x.Id == financialStatementId).ExecuteUpdateAsync(
                                         s => s
                                            .SetProperty(b => b.Name, fiananceStatementName)
                                            .SetProperty(b => b.StartDate, startDate)
                                            .SetProperty(b => b.EndDate, endDate),
                                        cancellationToken
                                    );
    }

    public async Task CreateFinanceAccountAsync(List<FinancialAccount> financialAccountList, List<FinancialStatementAccountMapping> financialStatementAccountMappingList, CancellationToken cancellationToken)
    {
        await _repositoryBase.AddRangeAsync<FinancialAccount>(financialAccountList, cancellationToken);
        await _repositoryBase.AddRangeAsync<FinancialStatementAccountMapping>(financialStatementAccountMappingList, cancellationToken);
        await _repositoryBase.SaveChangesAsync(cancellationToken);
    }

    public async Task<Guid> UpdateFinanceAccountAsync(Guid financialAccountId, DateTime? creditDate, string? accountName, Guid? bankAccountId, decimal? amount, CancellationToken cancellationToken)
    {
        IQueryable<FinancialAccount>? financialAccount = _repositoryBase.Query<FinancialAccount>(true).Where(x => x.IsActive && x.Id.Equals(financialAccountId));

        _ = await financialAccount.ExecuteUpdateAsync(
            s =>
                s.SetProperty(x => x.AccountName, x => accountName != null ? accountName : x.AccountName)
                .SetProperty(x => x.CreditDate, x => creditDate != null ? creditDate : x.CreditDate)
                .SetProperty(x => x.Amount, x => amount != null ? amount : x.Amount)
                .SetProperty(x => x.BankAccountId, x => bankAccountId != null ? bankAccountId : x.BankAccountId),
                cancellationToken
        );

        return financialAccountId;

    }

    public async Task<Guid> DeleteFinancialAccountAsync(Guid financialAccountId, CancellationToken cancellationToken)
    {
        IQueryable<FinancialAccount>? financialAccount = _repositoryBase.Query<FinancialAccount>(true).Where(x => x.IsActive && x.Id.Equals(financialAccountId));

        _ = await financialAccount.ExecuteUpdateAsync(
            s =>
            s.SetProperty(x => x.IsActive, false),
            cancellationToken
        );

        return financialAccountId;

    }

    public async Task<IQueryable<FinancialAccountOutputDto>> GetFinancialAccountAsync(Guid financialStatementId, CancellationToken cancellationToken)
    {
        IQueryable<FinancialAccountOutputDto>? query = (from fsfam in _repositoryBase.Query<FinancialStatementAccountMapping>(true).Where(x => x.IsActive && x.FinancialStatementId.Equals(financialStatementId))
                                                        join fs in _repositoryBase.Query<FinancialAccount>(true).Where(x => x.IsActive) on fsfam.FinancialAccountId equals fs.Id
                                                        join ba in _repositoryBase.Query<BankAccountDetail>(true).Where(x => x.IsActive) on fs.BankAccountId equals ba.Id
                                                        select new FinancialAccountOutputDto
                                                        {
                                                            Id = fs.Id,
                                                            AccountName = fs.AccountName,
                                                            Amount = fs.Amount,
                                                            AccountType = fs.AccountType,
                                                            CreditDate = fs.CreditDate,
                                                            BankAccountId = ba.BankName
                                                        });

        return query;
    }

    public async Task UpsertFinancialStatementAsync(
        List<FinancialAccountOutputDto> financialAccountOutputDtos,
        Guid financeStatementId,
        CancellationToken cancellationToken)
    {

        if (financialAccountOutputDtos == null || financialAccountOutputDtos.Count == 0)
        {
            return;
        }

        List<FinancialStatementAccountMapping> existingMappings = await _repositoryBase
            .Query<FinancialStatementAccountMapping>(true)
            .Where(x => x.FinancialStatementId == financeStatementId)
            .ToListAsync(cancellationToken);

        HashSet<Guid> mappedAccountIds = existingMappings
            .Select(x => x.FinancialAccountId)
            .ToHashSet();

        List<Guid> updateIds = financialAccountOutputDtos
            .Where(x => x.Id.HasValue && x.Id.Value != Guid.Empty)
            .Select(x => x.Id!.Value)
            .Distinct()
            .ToList();

        Dictionary<Guid, FinancialAccount> existingAccountsMap = updateIds.Count == 0
            ? new Dictionary<Guid, FinancialAccount>()
            : await _repositoryBase
                .Query<FinancialAccount>(false)
                .Where(x => updateIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, cancellationToken);

        DateTime now = DateTime.UtcNow;

        List<FinancialAccount> accountsToInsert = new();
        List<FinancialStatementAccountMapping> mappingsToInsert = new();

        foreach (FinancialAccountOutputDto dto in financialAccountOutputDtos)
        {
            if (!Guid.TryParse(dto.BankAccountId, out Guid bankAccountId))
            {
                continue;
            }

            FinancialAccount account;

            if (dto.Id.HasValue && dto.Id.Value != Guid.Empty)
            {
                if (!existingAccountsMap.TryGetValue(dto.Id.Value, out account))
                {
                    continue;
                }

                account.AccountName = dto.AccountName;
                account.Amount = dto.Amount;
                account.AccountType = dto.AccountType;
                account.CreditDate = dto.CreditDate;
                account.BankAccountId = bankAccountId;
                account.UpdatedAt = now;
            }
            else
            {
                account = new FinancialAccount
                {
                    Id = Guid.NewGuid(),
                    AccountName = dto.AccountName,
                    Amount = dto.Amount,
                    AccountType = dto.AccountType,
                    CreditDate = dto.CreditDate,
                    BankAccountId = bankAccountId,
                    CreatedAt = now
                };

                accountsToInsert.Add(account);
            }

            if (!mappedAccountIds.Contains(account.Id))
            {
                mappingsToInsert.Add(new FinancialStatementAccountMapping
                {
                    Id = Guid.NewGuid(),
                    FinancialAccountId = account.Id,
                    FinancialStatementId = financeStatementId,
                    CreatedAt = now
                });

                mappedAccountIds.Add(account.Id);
            }
        }

        if (accountsToInsert.Count > 0)
        {
            await _repositoryBase.AddRangeAsync(accountsToInsert, cancellationToken);
        }

        if (mappingsToInsert.Count > 0)
        {
            await _repositoryBase.AddRangeAsync(mappingsToInsert, cancellationToken);
        }

        await _repositoryBase.SaveChangesAsync(cancellationToken);

    }

    public async Task<List<GuidIdNameDto>> FetchBankAndAccountDetails(Guid guid, CancellationToken cancellationToken)
    {
        List<GuidIdNameDto> bankAccountdetail =await  ( from bm in _repositoryBase.FindByCondition<UserBankMapping>(x => x.IsActive && x.UserId.Equals(guid),true)
                                                  join b in _repositoryBase.FindByCondition<BankAccountDetail>(x => x.IsActive , true) on bm.BankAccountDetailId equals b.Id
                                                  select new GuidIdNameDto()
                                                  {
                                                      Id = b.Id,
                                                      Name = b.BankName
                                                  }
                                                ).ToListAsync(cancellationToken);
        return bankAccountdetail;

    }

}