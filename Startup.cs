
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Middlewares;
using UserManagement.Models;
using UserManagement.Services;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

        var secret = Configuration["AppSettings:Secret"];
        if (string.IsNullOrEmpty(secret))
        {
            throw new ArgumentNullException(nameof(secret), "Secret key cannot be null or empty");
        }

        var key = Encoding.ASCII.GetBytes(secret);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
        });

        // Role tabanlı yetkilendirme politikaları
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
        });

        services.AddScoped<IUserService, UserService>();
        services.AddControllers();
    }



    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<LoggingMiddleware>();

        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseMiddleware<AuthenticationMiddleware>();

        app.UseMiddleware<JwtMiddleware>();

        

        // app.UseWhen(context => !context.Request.Path.StartsWithSegments("/auth/login"), appBuilder =>
        // {
        //     //Console.WriteLine($"Request Path: {appBuilder}"); 
        //     appBuilder.UseMiddleware<AuthenticationMiddleware>();
        // });

        app.UseStaticFiles(); // Statik dosyalar için bu satırı ekleyin

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
