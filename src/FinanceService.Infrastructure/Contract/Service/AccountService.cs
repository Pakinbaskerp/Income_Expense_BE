using FinanceService.Application.Contract.IRepository;
using FinanceService.Domain.Dto;
using FinanceService.Domain.Models;
using FinanceService.Infrastructure.Contract.Repository;
using libraries.logging.Contract;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Infrastructure.Contract.Service;

public class AccountService(IRepositoryBase repoBase,
    ILoggerManager<AccountService> logger
    ) : IAccountService
{
    private readonly IRepositoryBase _repoBase = repoBase;
    private readonly ILoggerManager<AccountService> _logger = logger;

    public async Task CreateAccountDetailAsync(BankAccountDetail bankAccountDetail, UserBankMapping userBankMapping, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"Creating account detail for UserId: {userBankMapping.UserId}, BankAccountDetailId: {bankAccountDetail.Id}");
        await _repoBase.AddAsync<BankAccountDetail>(bankAccountDetail, cancellationToken);
        await _repoBase.AddAsync<UserBankMapping>(userBankMapping, cancellationToken);
        await _repoBase.SaveChangesAsync(cancellationToken);
        _logger.LogInfo($"Successfully created account detail for UserId: {userBankMapping.UserId}, BankAccountDetailId: {bankAccountDetail.Id}");
    }

    public async Task<List<AccountResponseDto>> GetAccountListForUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"Fetching account list for UserId: {userId}");
       IQueryable<AccountResponseDto> query =  (from usb in _repoBase.FindByCondition<UserBankMapping>(
                                                    x => x.IsActive && x.UserId == userId)

                                                join bad in _repoBase.FindByCondition<BankAccountDetail>(
                                                    b => b.IsActive)
                                                on usb.BankAccountDetailId equals bad.Id

                                                select new AccountResponseDto
                                                {
                                                    AccountId = bad.Id,
                                                    AccountName = bad.BankName,
                                                    IsCalculated = bad.IsCountable,
                                                    Balance = bad.Balance
                                                });
        _logger.LogInfo($"Successfully fetched account list for UserId: {userId}");

        return await query.ToListAsync(cancellationToken);
            
    }

    public async Task DeleteAccountDetailAsync(Guid bankAccountDetailId, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"Deleting account detail for BankAccountDetailId: {bankAccountDetailId}");

        IQueryable<BankAccountDetail>? bankAccountDetail =  _repoBase.Query<BankAccountDetail>(true);
        
        _ = bankAccountDetail.Where(x=> x.IsActive && x.Id == bankAccountDetailId).ExecuteUpdateAsync(
                                        s => s.SetProperty(b => b.IsActive, false),
                                        cancellationToken
                                    );
        IQueryable<UserBankMapping>? userBankMapping =  _repoBase.Query<UserBankMapping>(true);
        
        _ = userBankMapping.Where(x=> x.IsActive && x.BankAccountDetailId == bankAccountDetailId).ExecuteUpdateAsync(
                                        s => s.SetProperty(b => b.IsActive, false),
                                        cancellationToken
                                    );
        _logger.LogInfo($"Successfully deleted account detail for BankAccountDetailId: {bankAccountDetailId}");        
    }


    public async Task UpdateAccountDetailAsync(
        Guid bankAccountDetailId,
        string? accountName,
        long? balance,
        string? currencyCode,
        bool? isCalculate,
        CancellationToken cancellationToken)
    {
        _logger.LogInfo(
            "Updating account detail for BankAccountDetailId: {BankAccountDetailId}",
            bankAccountDetailId);
        
        // If nothing to update, just return
        if (string.IsNullOrWhiteSpace(accountName)
            && balance == null
            && isCalculate == null)
        {
            _logger.LogInfo(
                "No update fields provided for BankAccountDetailId: {BankAccountDetailId}",
                bankAccountDetailId);
            return;
        }

        IQueryable<BankAccountDetail> query =
            _repoBase.Query<BankAccountDetail>(asNoTracking: true)
                    .Where(x => x.IsActive && x.Id == bankAccountDetailId);
        
        _ = query.ExecuteUpdateAsync(
            sets => sets
                .SetProperty(b => b.BankName, b => accountName != null ? accountName : b.BankName)
                .SetProperty(b => b.Balance, b => balance != null ? balance.Value : b.Balance)
                .SetProperty(b => b.IsCountable, b => isCalculate != null ? isCalculate.Value : b.IsCountable),
            cancellationToken);
        _logger.LogInfo(
            "Successfully updated account detail for BankAccountDetailId: {BankAccountDetailId}",
            bankAccountDetailId);   
    }

}