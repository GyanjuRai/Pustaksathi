using Pustaksathi.Model.Shared.AppSettings;

namespace Pustaksathi.API.Middleware
{
    public static class Midlleware
    {
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
