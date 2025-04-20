using Pustaksathi.Interface.Shared.Account;
using Pustaksathi.Interface.Shared.Auth;
using Pustaksathi.Model.Shared.AppSettings;
using Pustaksathi.Services.Shared.Account;
using Pustaksathi.Services.Shared.Auth;

namespace Pustaksathi.API.Middleware
{
    public static class Midlleware
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>()
                    .AddScoped<IAuthService, AuthService>();

            return services;
        }

        public static IServiceCollection AppSettingConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(ServiceProvider =>
            {
                var appSetting = new AppSetting();
                configuration.GetSection("AppSetting").Bind(appSetting);
                return appSetting;
            });
            
            return services;
        }
    }
}
