using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceService.Domain.Models;

public class FinancialAccount : BaseModel
{
    public string AccountName { get; set; } = string.Empty;
    public Guid AccountType { get; set; }
    
    [ForeignKey("BankAccountDetail")]
    public Guid BankAccountId { get; set; }
    public BankAccountDetail? BankAccountDetail { get; set; }
    public DateTime CreditDate { get; set; }
    public decimal Amount { get; set; }
}