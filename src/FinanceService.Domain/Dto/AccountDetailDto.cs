namespace FinanceService.Infrastructure.Contract.Service;

public class AccountDetailDto
{
    public string BankName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public bool IsCountable { get; set; }
}