using FisioFinancials.Domain.Model.Interfaces.Repositories;
using FisioFinancials.Domain.Model.Interfaces.Services;
using FisioFinancials.Domain.Services.Services;
using FisioFinancials.Infrastructure.Data.Context;
using FisioFinancials.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace FisioFinancials.Infrastructure.IoC;

public sealed class DependencyInjectorHelper
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<IReceivedRepository, ReceivedRepository>();
        services.AddScoped<IReceivedService, ReceivedService>();

        services.AddDbContext<FisioFinancialsDbContext>(options =>
            options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=FisioFinancialsDB; Trusted_Connection=True"));
    }
}
