using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pustaksathi.API.Const;
using Pustaksathi.API.Middleware;
using Pustaksathi.Data.ApplicationDbContext;
using Pustaksathi.Model.Shared.Auth;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;


/**  
* ==============================  
*            Serilog       
* ==============================  
* **/
try
{
    Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The application failed to start correctly.");
}
finally
{
    Log.Information("Serilog Shutdown complete.");
    Log.CloseAndFlush();
}

/**
 * ===============================
 *     Service Configuration
 * ===============================
 */
var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .ReadFrom.Configuration(ctx.Configuration));

/**
 * ===============================
 *      Database connection
 * ===============================
 */
//string ENVIRONMENT = builder.Configuration.GetSection("Environment").Value ?? "Dev";
string CONNECTION_STRING = builder.Configuration[$"ConnectionString"] ?? builder.Configuration["ConnectionString"] ?? "";
builder.Services.AddDbContext<PustaksathiDbContext>(options => options.UseNpgsql(CONNECTION_STRING));

/**
 * ===============================
 *      Configure Cors Policy
 * ===============================
 */
string[]? allowOrigins = (builder.Configuration["AppSetting:Origins"] ?? "").Split(",");
builder.Services.AddCors(options =>
{
    options.AddPolicy(AppData.PolicyName,
        builder =>
        {
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();

            if (allowOrigins is not { Length: > 0 }) return;

            if (allowOrigins.Contains("*"))
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.SetIsOriginAllowed(host => true);
                builder.AllowCredentials();
            }
            else
            {
                builder.WithOrigins(allowOrigins)
                .AllowCredentials();
            }
        });
});

JwtTokenConfig jwtConfig = builder.Configuration.GetSection("Jwt").Get<JwtTokenConfig>() ?? new JwtTokenConfig();
builder.Services.AddSingleton(jwtConfig);

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwtConfig.Issuer,
        ValidateIssuer = true,
        ValidAudience = jwtConfig.Audience,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig?.Secret ?? "")),
        ClockSkew = TimeSpan.FromMinutes(jwtConfig?.AccessTokenClockSkewMin ?? 5)
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            context.Response.OnStarting(async () =>
            {
                context.NoResult();
                context.Response.Headers.Append("Token-Expired", "ture");
                context.Response.ContentType = "application/plain";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync(context.Exception.Message);
            });
            return Task.CompletedTask;
        }
    };
});

builder.Services
    .AppSettingConfig(builder.Configuration)
    .AddCoreServices();

builder.Services.AddControllers();

/**
 * ===============================
 *         API Versioning
 * ===============================
 */
builder.Services
    .AddApiVersioning(options =>
    {
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.ReportApiVersions = true;
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/**
 * =========================
 *      App Builder
 * =========================
 */
var app = builder.Build();

// Configure the HTTP request pipeline.  
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(AppData.PolicyName);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
