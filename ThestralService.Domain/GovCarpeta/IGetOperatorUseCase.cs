namespace ThestralService.Domain.GovCarpeta; 
using Dtos;

public interface IGetOperatorUseCase
{
    Task<(OperatorDto? requestedOperator, string replyTransferUrl)> GetOperatorAsync(string operatorId);
    Task<OperatorDto[]?> GetOperatorsAsync();
}
