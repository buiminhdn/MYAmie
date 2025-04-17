using Common.Mapping;
using Common.Validators.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Common;
public static class DependencyInjection
{
    public static IServiceCollection AddCommon(this IServiceCollection services)
    {
        services.AddSingleton<IValidationFactory, ValidationFactory>();

        services.AddAutoMapper(typeof(MappingProfile));

        return services;
    }
}
