using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TicketFlow_v2.Data;
using TicketFlow_v2.Models;
using TicketFlow_v2.Repository.Implementation;
using TicketFlow_v2.Repository.Interface;
using TicketFlow_v2.Services.Implementation;
using TicketFlow_v2.Services.Interface;
using TicketFlow_v2.Validators;

namespace TicketFlow_v2.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenApi();
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.AddDbContext<TicketDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("TicketingV2Connection")));

        services.Configure<SmtpMail>(configuration.GetSection("SmtpMail"));
        services.Configure<TelecomAbode>(configuration.GetSection("SmsSettings:TelecomAbode"));

        var smsProvider = configuration["SmsSettings:Provider"];
        if (!string.IsNullOrWhiteSpace(smsProvider) &&
            smsProvider.Equals("TelecomAbode", StringComparison.OrdinalIgnoreCase))
        {
            services.AddHttpClient<ISmsService, TelecomAbodeSmsService>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(60);
            });
        }
        else
        {
            services.AddSingleton<ISmsService, NullSmsService>();
        }

        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();

        return services;
    }
}
