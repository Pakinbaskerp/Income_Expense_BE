
using FinanceService.Infrastructure.Contract.Service;
using FluentValidation;
using libraries.logging.Contract;
using MediatR;

namespace FinanceService.Application.CQRS.AccountModule.Command;

public record UpdateAccountDetailCommand(Guid BankAccountDetailId, string? AccountName, long? Balance, string? CurrencyCode, bool? IsCalculate, CancellationToken CancellationToken) : IRequest<Guid>;

public class UpdateAccountDetailCommandValidator : AbstractValidator<UpdateAccountDetailCommand>
{
    public UpdateAccountDetailCommandValidator()
    {
        RuleFor(x => x.BankAccountDetailId).NotEmpty().WithMessage("Bank account detail ID must be provided.");
    }
}

public class UpdateAccountDetailCommandHandler : IRequestHandler<UpdateAccountDetailCommand, Guid>
{
    private readonly IAccountService _accountService;
    private readonly ILoggerManager<UpdateAccountDetailCommandHandler> _logger;

    public UpdateAccountDetailCommandHandler(IAccountService accountService,
        ILoggerManager<UpdateAccountDetailCommandHandler> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    public async Task<Guid> Handle(UpdateAccountDetailCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"Updating bank account detail with ID: {request.BankAccountDetailId}");
        await _accountService.UpdateAccountDetailAsync(request.BankAccountDetailId, request.AccountName, request.Balance, request.CurrencyCode, request.IsCalculate, cancellationToken);
        _logger.LogInfo($"Successfully updated bank account detail with ID: {request.BankAccountDetailId}");
        return request.BankAccountDetailId;
    }
}
