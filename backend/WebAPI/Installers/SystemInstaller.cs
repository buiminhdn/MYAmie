using BLL;
using Common;
using Common.DTOs.Core;
using DAL;
using FluentValidation;
using MYAmie.WebAPI.Services.Files;
using WebAPI.Filters;
using WebAPI.Installers.Core;
using WebAPI.Services.Files;

namespace WebAPI.Installers;

public class SystemInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IFileService, FileService>();
        services.AddDAL(configuration);
        services.AddBLL(configuration);
        services.AddCommon();

        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;

        services.AddControllers(options =>
        {
            options.Filters.Add(new AutoAddUserFilter(typeof(BaseParams)));
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
