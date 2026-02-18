namespace FinanceService.Infrastructure.Contract.Service;

public class UpdateAccountDetailDto
{
    public Guid BankAccountDetailId { get; set; }
    public string? AccountName { get; set; }
    public long? Balance { get; set; }
    public string? CurrencyCode { get; set; }
    public bool? IsCalculate { get; set; }
}