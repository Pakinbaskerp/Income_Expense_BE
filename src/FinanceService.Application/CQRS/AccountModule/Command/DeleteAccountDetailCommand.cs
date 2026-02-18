using FinanceService.Infrastructure.Contract.Service;
using FluentValidation;
using libraries.logging.Contract;
using MediatR;

namespace FinanceService.Application.CQRS.AccountModule.Command;

public record DeleteAccountDetailCommand(Guid BankAccountDetailId, CancellationToken CancellationToken):IRequest<Guid>;

public class DeleteAccountDetailCommandValidator : AbstractValidator<DeleteAccountDetailCommand>
{
    public DeleteAccountDetailCommandValidator()
    {
        RuleFor(x => x.BankAccountDetailId).NotEmpty().WithMessage("Bank account detail ID must be provided.");
    }
}
public class DeleteAccountDetailCommandHandler : IRequestHandler<DeleteAccountDetailCommand, Guid>
{
    private readonly IAccountService _accountService;
    private readonly ILoggerManager<DeleteAccountDetailCommandHandler> _logger;

    public DeleteAccountDetailCommandHandler(IAccountService accountService, 
        ILoggerManager<DeleteAccountDetailCommandHandler> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    public async Task<Guid> Handle(DeleteAccountDetailCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"Deleting bank account detail with ID: {request.BankAccountDetailId}");
        await _accountService.DeleteAccountDetailAsync(request.BankAccountDetailId, cancellationToken);
        _logger.LogInfo($"Successfully deleted bank account detail with ID: {request.BankAccountDetailId}");
        return request.BankAccountDetailId;
    }
}