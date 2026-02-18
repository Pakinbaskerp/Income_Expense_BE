
namespace FinanceService.Domain.Dto;

public class  FinanceStateDisplayDetailDto
{
    public List<FinancialAccountOutputDto> FinancialAccountOutputDto{ get; set; } = new ();
    public List<GuidIdNameDto> BankAccountNameDetail { get; set; } = new List<GuidIdNameDto>();
    public List<GuidIdNameDto> AccountTypeDetail { get; set; } = new ();
}