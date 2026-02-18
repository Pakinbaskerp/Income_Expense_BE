
using FinanceService.Application.Contract.IService;
using FinanceService.Domain.Dto;
using FluentValidation;
using libraries.logging.Contract;
using MediatR;

public record GetFinanceStatementAccountQuery(Guid AccountId, Guid TaxYearId, CancellationToken CancellationToken) : IRequest<List<FinanceAccountResponseDto>>;
public class GetFinanceStatementAccountQueryValidator : AbstractValidator<GetFinanceStatementAccountQuery>
{
    public GetFinanceStatementAccountQueryValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty().WithMessage("AccountId is required.");
        RuleFor(x => x.TaxYearId).NotEmpty().WithMessage("TaxYearId is required.");
    }
}

public class GetFinanceStatementAccountQueryHandler : IRequestHandler<GetFinanceStatementAccountQuery, List<FinanceAccountResponseDto>>
{
    private readonly IFinanceStatementService _financeStatementService;
    private readonly ILoggerManager<GetFinanceStatementAccountQueryHandler> _logger;
    public GetFinanceStatementAccountQueryHandler(
        IFinanceStatementService financeStatementService,
        ILoggerManager<GetFinanceStatementAccountQueryHandler> logger
    )
    {
        _financeStatementService = financeStatementService;
        _logger = logger;
    }
    public async Task<List<FinanceAccountResponseDto>> Handle(GetFinanceStatementAccountQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"Handling GetFinanceStatementAccountQuery for AccountId: {request.AccountId}");
        IEnumerable<FinanceAccountResponseDto>? result = await _financeStatementService.GetFinancialStatementsAsync(request.AccountId, request.TaxYearId, request.CancellationToken);
        _logger.LogInfo($"Retrieved {result.Count()} finance accounts for AccountId: {request.AccountId}");
        return result.ToList() 
           ?? throw new Exception("Finance account not found");
    }
}
