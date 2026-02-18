namespace FinanceService.Domain.Models;

public class ProviderDetail : BaseModel
{
    public string ProviderName { get; set; } = string.Empty;
    public string ProviderSubId { get; set; } = string.Empty;
}