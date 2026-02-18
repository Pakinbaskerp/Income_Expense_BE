using FinanceService.Domain.Dto;
using FinanceService.Infrastructure.Contract.Service;
using FluentValidation;
using libraries.logging.Contract;
using MediatR;

namespace FinanceService.Application.CQRS.AccountModule;

public record GetAccountListForUserQuery(Guid UserId) : IRequest<List<AccountResponseDto>>;
public class GetAccountListForUserQueryValidator : AbstractValidator<GetAccountListForUserQuery>
{
    public GetAccountListForUserQueryValidator()
    {
        RuleFor(x=> x.UserId)
        .NotEmpty().WithMessage("User id should not be empty");
    }    
}

public class GetAccountListForUserQueryHandler : IRequestHandler<GetAccountListForUserQuery, List<AccountResponseDto>>
{
    private readonly IAccountService    _accountService;
    private readonly ILoggerManager<GetAccountListForUserQueryHandler> _logger;
    
    public GetAccountListForUserQueryHandler(IAccountService accountService, ILoggerManager<GetAccountListForUserQueryHandler> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }
    public Task<List<AccountResponseDto>> Handle(GetAccountListForUserQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"Handling GetAccountListForUserQuery for UserId: {request.UserId}");
        return _accountService.GetAccountListForUserAsync(request.UserId, cancellationToken);
    }
}