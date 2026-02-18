namespace FinanceService.Application.Contract.IService;

public interface IUserContext
{
    Guid UserId { get; }
   bool IsAuthenticated { get; }
}