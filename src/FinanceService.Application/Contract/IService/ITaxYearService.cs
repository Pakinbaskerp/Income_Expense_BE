using FinanceService.Domain.Dto;
using FinanceService.Domain.Models;

namespace FinanceService.Application.Contract.IService;
public interface ITaxYearService
{
    Task CreateTaxYearAsync(TaxYear taxYear, CancellationToken cancellationToken);
    Task<List<GuidIdNameDto>> GetAllTaxYearList();
}