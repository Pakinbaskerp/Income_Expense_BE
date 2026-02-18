namespace FinanceService.Domain.Models;

public class User : BaseModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PictureUrl { get; set; } = string.Empty;
    
}