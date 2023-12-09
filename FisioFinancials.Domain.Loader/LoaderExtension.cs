using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FisioFinancials.Domain.Loader;

public static class LoaderExtension
{
    public static void RunLoaders(this IApplicationBuilder app)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        List<Type> loaderTypes = new();

        Type loaderBaseType = typeof(Base);

        foreach (Assembly assembly in assemblies)
        {
            IEnumerable<Type> loaderTypesEnumerable = assembly.GetTypes()
                .Where(type => !type.IsAbstract && type.IsSubclassOf(loaderBaseType));
            loaderTypes.AddRange(loaderTypesEnumerable);
        }

        using IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

        for (int i = 0; i < loaderTypes.Count; i++)
        {
            var loader = (Base)ActivatorUtilities.CreateInstance(serviceScope.ServiceProvider, loaderTypes[i]);
            loader.ExecuteAsync().Wait();
        }
    }
}