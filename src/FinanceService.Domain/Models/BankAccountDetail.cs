namespace FinanceService.Domain.Models;

public class BankAccountDetail : BaseModel
{
    public string BankName { get; set; } = string.Empty;
    public Guid BankType { get; set; }
    public long Balance { get; set; } 
    public Guid CurrencyCode { get; set; } 
    public bool IsCountable { get; set; }
}