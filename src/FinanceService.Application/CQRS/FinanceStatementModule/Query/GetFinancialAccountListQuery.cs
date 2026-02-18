using FinanceService.Application.Contract.IService;
using FinanceService.Domain.Dto;
using libraries.logging.Contract;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Application.CQRS.FinanceStatementModule.Query;

public sealed record GetFinancialAccountListQuery(
    Guid FinancialStatementId,
    CancellationToken CancellationToken
) : IRequest<FinanceStateDisplayDetailDto>;

public class GetFinancialAccountListQueryHandler : IRequestHandler<GetFinancialAccountListQuery, FinanceStateDisplayDetailDto>
{
    private readonly IFinanceStatementService _financeStatementService;
    private readonly ILoggerManager<GetFinancialAccountListQueryHandler> _logger;
    public GetFinancialAccountListQueryHandler(
        IFinanceStatementService financeStatementService,
        ILoggerManager<GetFinancialAccountListQueryHandler> logger
    )
    {
        _financeStatementService = financeStatementService;
        _logger = logger;
    }
    public async Task<FinanceStateDisplayDetailDto> Handle(GetFinancialAccountListQuery request, CancellationToken cancellationToken)
    {
        IQueryable<FinancialAccountOutputDto>? financialAccountsStatements = await _financeStatementService.GetFinancialAccountAsync(financialStatementId: request.FinancialStatementId, cancellationToken);

        List<GuidIdNameDto> bankAccountDetails = await _financeStatementService.FetchBankAndAccountDetails(Guid.Parse("E256BD22-5BA5-4C56-B9BF-BCE906209949"), cancellationToken);

        return new()
        {
            FinancialAccountOutputDto = financialAccountsStatements.ToList(),
            BankAccountNameDetail = bankAccountDetails,
            AccountTypeDetail = getAccountTypes()
        };
    }

    public List<GuidIdNameDto> getAccountTypes()
    {
        List<GuidIdNameDto> AccountTypeDetail = new()
        {
            new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Income" },
            new() { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Food" },
            new() { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Travel" },
            new() { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Movie / Entertainment" },
            new() { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "Shopping" },
            new() { Id = Guid.Parse("66666666-6666-6666-6666-666666666666"), Name = "Health / Medical" },
            new() { Id = Guid.Parse("77777777-7777-7777-7777-777777777777"), Name = "Education" },
            new() { Id = Guid.Parse("88888888-8888-8888-8888-888888888888"), Name = "Rent / House" },
            new() { Id = Guid.Parse("99999999-9999-9999-9999-999999999999"), Name = "Utilities (EB / Water / Gas)" },
            new() { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "Mobile / Internet" },
            new() { Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Name = "Fuel / Petrol" },
            new() { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), Name = "Groceries" },
            new() { Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), Name = "Insurance" },
            new() { Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), Name = "EMI / Loan" },
            new() { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Name = "Investment" },
            new() { Id = Guid.Parse("12121212-1212-1212-1212-121212121212"), Name = "Gift / Donation" },
            new() { Id = Guid.Parse("13131313-1313-1313-1313-131313131313"), Name = "Maintenance" },
            new() { Id = Guid.Parse("14141414-1414-1414-1414-141414141414"), Name = "Subscription (OTT / Apps)" },
            new() { Id = Guid.Parse("15151515-1515-1515-1515-151515151515"), Name = "Other" }
        };
        return AccountTypeDetail;
    }
}