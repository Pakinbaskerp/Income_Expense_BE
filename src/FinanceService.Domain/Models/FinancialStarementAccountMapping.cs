using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceService.Domain.Models;

public class FinancialStatementAccountMapping : BaseModel
{
    [ForeignKey("FinancialAccount")]
    public Guid FinancialAccountId { get; set; }
    public FinancialAccount? FinancialAccount { get; set; }

    [ForeignKey("FinancialStatement")]
    public Guid FinancialStatementId { get; set; }
    public FinancialStatement? FinancialStatement { get; set; }
}