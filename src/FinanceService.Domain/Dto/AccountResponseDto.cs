namespace FinanceService.Domain.Dto;

public class AccountResponseDto
{
    public Guid AccountId { get; set; }
    public string? AccountName { get; set; }
    public long? Balance { get; set; }
    public bool? IsCalculated { get; set; }
}