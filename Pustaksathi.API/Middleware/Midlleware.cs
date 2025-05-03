using Pustaksathi.Interface.Application.Admin;
using Pustaksathi.Interface.Application.Books;
using Pustaksathi.Interface.Application.Members;
using Pustaksathi.Interface.Application.Staff;
using Pustaksathi.Interface.Shared.Account;
using Pustaksathi.Interface.Shared.Auth;
using Pustaksathi.Model.Shared.AppSettings;
using Pustaksathi.Services.Application.Admin;
using Pustaksathi.Services.Application.Books;
using Pustaksathi.Services.Application.Members;
using Pustaksathi.Services.Application.Staff;
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

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IBooksServices, BooksServices>();
            services.AddTransient<IStaffService, StaffService>();
            services.AddTransient<IAdminSerivce, AdminService>();
            services.AddTransient<IMembersService, MembersService>();

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
