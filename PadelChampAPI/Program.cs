using PadelChampAPI.Extensions;

namespace PadelChampAPI;

public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder
            .Services.AddControllersAndSwagger()
            .AddConnection(builder.Configuration)
            .AddAuthenticationAuthorizationAndIdentity(builder.Configuration)
            .AddRepository()
            .AddCorsExtension().AddStripe(builder.Configuration);

        var Configuration = builder.Configuration;

        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

        var app = builder.Build();
        app.UseStaticFiles();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        /*using (var scope = app.Services.CreateScope())
        {
            try
            {
                var Services = scope.ServiceProvider;
                var context = Services.GetRequiredService<AppDbContext>();
                context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                var LoggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, ex.Message);
            }
        }*/

        //app.UseHttpsRedirection();
        app.UseCors("CorsPolicy");
        app.UseAuthorization();

        app.MapControllers();

        app.Run();

    }
}
