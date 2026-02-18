namespace FinanceService.Domain.Dto;

public class FinancialAccountOutputDto
{
    public Guid? Id { get; set; }
    public string? AccountName { get; set; }
    public Decimal Amount { get; set;}
    public Guid AccountType { get; set; }
    public DateTime CreditDate { get; set; }
    public string BankAccountId { get; set; } = string.Empty;
}