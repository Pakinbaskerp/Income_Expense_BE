

namespace FinanceService.Domain.Dto;

public sealed class PasswordHashingOptions
{
    public int SaltSize { get; set; }
    public int KeySize { get; set; }
    public int Iterations { get; set; }
    public string Algorithm { get; set; } = "SHA256";
    public string PepperKey { get; set; } = string.Empty;

}
