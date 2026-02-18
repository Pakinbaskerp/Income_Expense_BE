namespace FinanceService.Domain.Dto;

public class FinancialStatementUpsertDto
{
    public List<FinancialAccountOutputDto> financialAccountOutputDtos { get; set; } = new List<FinancialAccountOutputDto>();
    public Guid FinanceStatementId { get; set;}
}