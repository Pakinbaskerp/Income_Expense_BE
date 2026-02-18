using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceService.Domain.Models;

public class UserLock : BaseModel
{
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public long LockCount { get; set; }
    public DateTime LastFailedAttempt { get; set; }
    public bool IsLocked { get; set; }

}