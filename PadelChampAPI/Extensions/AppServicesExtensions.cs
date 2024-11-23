using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PadelChampAPI.Data;
using PadelChampAPI.Interfaces;
using PadelChampAPI.Models;
using PadelChampAPI.Repositories;
using PadelChampAPI.Services;
using Stripe;

namespace PadelChampAPI.Extensions;

public static class AppServicesExtensions
{
    //! Extension method for database connection
    //this IServiceCollection services, IConfiguration config
    public static IServiceCollection AddConnection(
        this IServiceCollection service,
        IConfiguration config
    )
    {
        service.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        });
        return service;
    }

    public static IServiceCollection AddControllersAndSwagger(this IServiceCollection service)
    {
        service.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        service.AddEndpointsApiExplorer();
        service.AddSwaggerGen();
        return service;
    }

    public static IServiceCollection AddAuthenticationAuthorizationAndIdentity(
        this IServiceCollection service,
        IConfiguration config
    )
    {
        // Add the Authentication Service
        service.AddScoped<ICustomAuthenticationService, AuthenticatoinService>();
        service.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        service.Configure<JwtSettings>(config.GetSection("JwtSettings"));

        // Configure the Interfaces for the Identity
        service
            .AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>() // this adds the implementation of the interfaces
            .AddDefaultTokenProviders();

        service
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                string? jwtIssuer = config.GetSection("JwtSettings:Issuer").Value;
                string? jwtKey = config.GetSection("JwtSettings:Key").Value;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!)),
                };
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = "MyCookie";
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;
            });
        return service;
    }

    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        //services.AddScoped(typeof(IGenericRepositoryMapped<,>), typeof(GenericRepositoryMapped<,>));
        return services;
    }

    public static IServiceCollection AddCorsExtension(this IServiceCollection services)
    {
        // allowing CORS
        services.AddCors(options =>
        {
            options.AddPolicy(
                "CorsPolicy",
                builder =>
                {
                    builder
                        .WithOrigins("https://localhost:7253")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithExposedHeaders("x-Authorization") // Expose custom headers if needed
                        .SetIsOriginAllowed(_ => true)
                        .WithHeaders("Content-Type"); // Allow Content-Type header
                }
            );
        });
        return services;
    }

    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }

    public static IServiceCollection AddSeedService(this IServiceCollection services)
    {
        //services.AddScoped(typeof(IGenericSeedService<,>), typeof(GenericSeedService<,>));
        return services;
    }
    public static IServiceCollection AddStripe(this IServiceCollection services,IConfiguration Configuration)
    {
        StripeConfiguration.ApiKey = Configuration["Stripe:SecretKey"];
        services.AddScoped(typeof(PaymentService));
        return services;
    }
}
