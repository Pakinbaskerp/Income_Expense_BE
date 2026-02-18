using FinanceService.Application.Contract.IService;
using FinanceService.Domain.Models;
using libraries.logging.Contract;
using MediatR;

namespace FinanceService.Infrastructure.Contract.Service;

public record CreateFinanceStatementCommand(
    Guid UserId,
    string AccountName,
    DateTime StartDate,
    DateTime EndDate,
    Guid CurrencyCode,
    Guid TaxYearId,
    CancellationToken CancellationToken
) : IRequest<Guid>;

public class CreateFinanceStatementCommandHandler : IRequestHandler<CreateFinanceStatementCommand, Guid>
{
    private readonly IFinanceStatementService _financeStatementService;
    private readonly ILoggerManager<CreateFinanceStatementCommandHandler> _logger;

    public CreateFinanceStatementCommandHandler(
        IFinanceStatementService financeStatementService,
        ILoggerManager<CreateFinanceStatementCommandHandler> logger
    )
    {
        _financeStatementService = financeStatementService;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateFinanceStatementCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo("Handling CreateFinanceStatementCommand");

        FinancialStatement financialStatement = new FinancialStatement
        {
            Id = Guid.NewGuid(),
            Name = request.AccountName,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            TaxYearId = request.TaxYearId,
            UserId = request.UserId
        };

        UserFinancialStatementMapping userFinancialStatementMapping = new UserFinancialStatementMapping
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            FinancialStatementId = financialStatement.Id
        };

        await _financeStatementService.CreateFinancialStatementAsync(userFinancialStatementMapping, financialStatement, request.CancellationToken);

        _logger.LogInfo($"Successfully created financial statement with ID: {financialStatement.Id}");

        return financialStatement.Id;
    }
}