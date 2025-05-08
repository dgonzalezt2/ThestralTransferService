using ThestralService.Domain.GovCarpeta.Dtos;

namespace ThestralService.Infrastructure.GovCarpeta;

public interface IOperatorsHttpClient
{
    Task<OperatorDto[]?> ExecuteAsync();
}
