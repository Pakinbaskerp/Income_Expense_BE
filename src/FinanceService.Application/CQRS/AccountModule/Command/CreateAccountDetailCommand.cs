using FinanceService.Domain.Models;
using FinanceService.Infrastructure.Contract.Service;
using FluentValidation;
using libraries.logging.Contract;
using MediatR;

namespace FinanceService.Application.CQRS.AccountModule.Command;

public record CreateAccountDetailCommand(
    Guid UserId,
    string AccountName,
    string AccountType,
    decimal InitialBalance,
    bool IsCalculated,
    CancellationToken CancellationToken
) : IRequest<Guid>;

public class CreateAccountDetailCommandValidator : AbstractValidator<CreateAccountDetailCommand>
{
    public CreateAccountDetailCommandValidator()
    {
        RuleFor(x => x.AccountName).NotEmpty().MaximumLength(20);
        RuleFor(x => x.AccountType).NotEmpty().MaximumLength(50);
        RuleFor(x => x.InitialBalance).GreaterThanOrEqualTo(0);
    }
}

public class CreateAccountDetailCommandHandler : IRequestHandler<CreateAccountDetailCommand, Guid>
{
    private readonly IAccountService _accountService;
    private readonly ILoggerManager<CreateAccountDetailCommandHandler> _logger;
    public CreateAccountDetailCommandHandler(
        IAccountService accountService, ILoggerManager<CreateAccountDetailCommandHandler> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateAccountDetailCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"Handling CreateAccountDetailCommand for UserId: {request.UserId}, AccountName: {request.AccountName}");
       BankAccountDetail bankAccountDetail = new ()
       {
            Id = Guid.NewGuid(),
            BankName = request.AccountName,
            BankType = Guid.NewGuid(),
            Balance = (long)(request.InitialBalance), 
            CurrencyCode = Guid.NewGuid(),
            IsCountable = request.IsCalculated
           
       };
        _logger.LogInfo($"Created BankAccountDetail with Id: {bankAccountDetail.Id}");
       UserBankMapping userBankMapping = new ()
       {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            BankAccountDetailId = bankAccountDetail.Id
       };
        _logger.LogInfo($"Created UserBankMapping with Id: {userBankMapping.Id} for UserId: {request.UserId}");
       await _accountService.CreateAccountDetailAsync(bankAccountDetail, userBankMapping, cancellationToken);
        _logger.LogInfo($"Successfully handled CreateAccountDetailCommand for UserId: {request.UserId}, AccountName: {request.AccountName}");
       return bankAccountDetail.Id;

        
    }
}