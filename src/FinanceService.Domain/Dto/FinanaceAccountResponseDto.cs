
namespace FinanceService.Domain.Dto;
public class FinanceAccountResponseDto
{
    public string AccountName { get; set; } = string.Empty;
    public Guid AccountId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}