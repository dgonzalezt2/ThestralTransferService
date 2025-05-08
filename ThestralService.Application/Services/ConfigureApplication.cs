namespace ThestralService.Application.Services;

using Interfaces;
using UseCases;
using Microsoft.Extensions.DependencyInjection;

public static class ConfigureApplication
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICreateTransferUseCase, CreateTransferUseCase>();
        services.AddScoped<ITransferToExternalProviderUseCase, TransferToExternalProviderUseCase>();
        typeof(ConfigureApplication).Assembly
            .GetTypes()
            .Where(uc =>
                uc.GetInterfaces().Any(i => i.Name == nameof(IInternalServicesResponseUseCase)))
            .ToList()
            .ForEach(useCase =>
            {
                var serviceType = useCase.GetInterfaces().First(i => i.Name == nameof(IInternalServicesResponseUseCase));
                services.AddScoped(serviceType, useCase);
            });
    }
}
