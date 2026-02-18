namespace FinanceService.Domain.Models;

public class Contact : BaseModel
{
    public string PhoneNumber { get; set; } = string.Empty;
    public Guid MobileCode { get; set; }
    public string AlternatePhoneNumber { get; set; } = string.Empty;
    public string AlternateEmail { get; set; } = string.Empty;
} 