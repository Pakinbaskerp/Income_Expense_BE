using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceService.Domain.Models;

public class UserBankMapping : BaseModel
{
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User? User { get; set; }

    [ForeignKey("BankAccountDetail")]
    public Guid BankAccountDetailId { get; set; }
    public BankAccountDetail? BankAccountDetail { get; set; }
}