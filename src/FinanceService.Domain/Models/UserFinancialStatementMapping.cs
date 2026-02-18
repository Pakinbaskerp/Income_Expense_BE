namespace FinanceService.Domain.Models;

public class UserFinancialStatementMapping : BaseModel
{
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Guid FinancialStatementId { get; set; }
    public FinancialStatement? FinancialStatement { get; set; }
}