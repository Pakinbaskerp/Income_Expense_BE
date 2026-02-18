namespace FinanceService.Domain.Dto;

public class FinanceAccountRequestDto
{
    public Guid? AccountId { get; set; }
    public string? AccountName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; } 
    public Guid CurrencyCode { get; set; }
}