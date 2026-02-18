namespace FinanceService.Domain.Dto;

public class FinanceAccountInputDto
{
    public string? AccountName { get; set; }
    public Guid AccountType { get; set; }
    public Guid BankAccountId { get; set; }
    public DateTime CreditDate { get; set; }
    public long Amount { get; set; }
}