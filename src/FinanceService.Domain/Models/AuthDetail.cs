using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceService.Domain.Models;

public class AuthDetail : BaseModel
{
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string? PasswordHash { get; set; } = string.Empty;
    public string? PasswordSalt { get; set; } = string.Empty;
    [ForeignKey("ProviderDetail")]
    public Guid? ProviderId { get; set; }
    public ProviderDetail? ProviderDetail { get; set; }
    public DateTime? PasswordChangedAt { get; set; }
}