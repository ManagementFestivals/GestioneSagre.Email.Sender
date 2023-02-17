using GestioneSagre.Email.Sender.BusinessLayer.Handlers;
using GestioneSagre.Email.Sender.BusinessLayer.Services;
using GestioneSagre.Email.Sender.DataAccessLayer;
using GestioneSagre.Email.Sender.Shared.Options;
using MediatR;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace GestioneSagre.Email.Sender;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddCors(options =>
        {
            options.AddPolicy("GestioneSagre.Email.Sender", policy =>
            {
                policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Gestione Sagre Email Sender",
                Version = "v1"
            });
        });

        services.AddDbContextPool<EmailSenderDbContext>(optionsBuilder =>
        {
            var connectionString = Configuration.GetSection("ConnectionStrings").GetValue<string>("Default");

            optionsBuilder.UseSqlServer(connectionString, options =>
            {
                //Creazione migration: Add-Migration NOME-MIGRATION -Project GestioneSagre.Email.Sender.Migrations
                //Esecuzione migration: Update-Database
                options.MigrationsAssembly("GestioneSagre.Email.Sender.Migrations");

                // Abilito il connection resiliency per gestire le connessioni perse
                // Info su: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
                options.EnableRetryOnFailure(3);
            });
        });

        services.AddSingleton<IEmailSender, MailKitEmailSender>();
        services.AddSingleton<IEmailClient, MailKitEmailSender>();

        services.Configure<KestrelServerOptions>(Configuration.GetSection("Kestrel"));
        services.Configure<SmtpOptions>(Configuration.GetSection("Smtp"));

        services.AddMediatR(typeof(SendEmailHandler).Assembly);
    }

    public void Configure(WebApplication app)
    {
        IWebHostEnvironment env = app.Environment;

        app.UseCors("GestioneSagre.Email.Sender");

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Gestione Sagre Email Sender v1");
        });

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}