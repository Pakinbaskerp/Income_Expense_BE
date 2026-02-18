using FinanceService.Domain.Dto;
using FinanceService.Domain.Models;

namespace FinanceService.Infrastructure.Contract.Service;

public interface IAccountService
{
    Task CreateAccountDetailAsync(BankAccountDetail bankAccountDetail, UserBankMapping userBankMapping, CancellationToken cancellationToken);
    Task<List<AccountResponseDto>> GetAccountListForUserAsync(Guid userId, CancellationToken cancellationToken);
    Task DeleteAccountDetailAsync(Guid bankAccountDetailId, CancellationToken cancellationToken);
    Task UpdateAccountDetailAsync(Guid bankAccountDetailId, string? accountName, long? balance, string? currencyCode, bool? isCalculate, CancellationToken cancellationToken);
}