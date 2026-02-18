using System.Threading.Tasks;
using FinanceService.Application.Contract.IRepository;
using FinanceService.Application.Contract.IService;
using FinanceService.Domain.Dto;
using FinanceService.Domain.Models;

namespace FinanceService.Infrastructure.Contract.Service;

public class TaxYearService : ITaxYearService
{
    private readonly IRepositoryBase _repositoryBase;
    public TaxYearService(
        IRepositoryBase repositoryBase
    )
    {
     _repositoryBase = repositoryBase;   
    }
    public async Task CreateTaxYearAsync(TaxYear taxYear, CancellationToken cancellationToken)
    {
        await _repositoryBase.AddAsync<TaxYear>(taxYear, cancellationToken);
        await _repositoryBase.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<GuidIdNameDto>> GetAllTaxYearList()
    {
       return _repositoryBase.Query<TaxYear>(true)
                          .Where(x => x.IsActive)
                          .Select(x => new GuidIdNameDto
                          {
                              Id = x.Id,
                              Name = x.Year
                          })
                          .ToList();
    }
}