using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceService.Domain.Models;

public class FinancialStatement : BaseModel
{
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User? User { get; set; }

    [ForeignKey("TaxYear")]
    public Guid TaxYearId { get; set; }
    public TaxYear? TaxYear { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid CurrencyCode { get; set; }
}