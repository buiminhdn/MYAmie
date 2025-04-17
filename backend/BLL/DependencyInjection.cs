using BLL.Interfaces;
using BLL.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLL;
public static class DependencyInjection
{
    public static IServiceCollection AddBLL(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IPlaceService, PlaceService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IFeedbackService, FeedbackService>();
        services.AddScoped<IAdminPlaceService, AdminPlaceService>();
        services.AddScoped<IAdminUserService, AdminUserService>();
        services.AddScoped<IFriendshipService, FriendshipService>();

        var mailerSendOptions = configuration.GetRequiredSection("MailerSend");
        var apiToken = mailerSendOptions["ApiToken"];
        var SenderName = mailerSendOptions["SenderName"];
        var SenderEmail = mailerSendOptions["SenderEmail"];

        services.AddMailerSend(options =>
        {
            options.ApiToken = apiToken;
            options.SenderName = SenderName;
            options.SenderEmail = SenderEmail;
        });

        return services;
    }
}
