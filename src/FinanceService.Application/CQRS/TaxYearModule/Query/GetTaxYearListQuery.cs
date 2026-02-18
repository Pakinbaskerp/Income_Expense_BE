using FinanceService.Application.Contract.IService;
using FinanceService.Domain.Dto;
using MediatR;

namespace FinanceService.Application.CQRS.TaxYearModule.Query;

public sealed record GetTaxYearListQuery(
CancellationToken CancellationToken
) : IRequest<List<GuidIdNameDto>>;

public class GetTaxYearListQueryHandler : IRequestHandler<GetTaxYearListQuery, List<GuidIdNameDto>>
{
    private readonly ITaxYearService _taxYearService;
    public GetTaxYearListQueryHandler(
        ITaxYearService taxYearService
    )
    {
        _taxYearService = taxYearService;
    }
    public Task<List<GuidIdNameDto>> Handle(GetTaxYearListQuery request, CancellationToken cancellationToken)
    {
        return _taxYearService.GetAllTaxYearList();
    }
}