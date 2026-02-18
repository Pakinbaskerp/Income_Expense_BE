namespace FinanceService.Domain.Models;

public class RefreshToken : BaseModel
{
    public string TokenHash { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }
}