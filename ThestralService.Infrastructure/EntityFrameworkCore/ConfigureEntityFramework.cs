namespace ThestralService.Infrastructure.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;
using Commands;
using Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DbContext;
using Domain.Transfer.Repositories;

internal static class ConfigureEntityFramework
{
    public static void AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TransferDbContext>(options => {
            options.UseNpgsql(configuration.GetConnectionString("PostgresSql"));
        });
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITransferCommandRepository, TransferCommandRepository>();
        services.AddScoped<ITransferQueryRepository, TransferQueryRepository>();
    }
}
