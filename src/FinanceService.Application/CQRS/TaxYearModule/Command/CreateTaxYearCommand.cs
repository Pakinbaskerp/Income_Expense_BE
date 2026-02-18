using FinanceService.Application.Contract.IService;
using FinanceService.Domain.Models;
using MediatR;

namespace FinanceService.Application.CQRS.TaxYearModule;
public record CreateTaxYearCommand(string TaxYear, CancellationToken CancellationToken) : IRequest<Guid>;
public class CreateTaxYearCommandHandler : IRequestHandler<CreateTaxYearCommand, Guid>
{
    private readonly ITaxYearService _taxYearService;
    public CreateTaxYearCommandHandler(
        ITaxYearService taxYearService
    )
    {
        _taxYearService = taxYearService;
    }
    public async Task<Guid> Handle(CreateTaxYearCommand request, CancellationToken cancellationToken)
    {
        TaxYear taxYear = new()
        {
            Year = request.TaxYear
        };

        await _taxYearService.CreateTaxYearAsync(taxYear, cancellationToken);
        return taxYear.Id;

    }
}