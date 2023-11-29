using Application.Extensions;
using Data.Models;
using Serilog;
using Worker.Consumers.Notification;
using Worker.Extensions;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        const string outputTemplate =
            "[{Timestamp:HH:mm:ss} {Level:u3}] [{ParentId}] {Message:lj}{NewLine}{Exception}";

        services.AddSerilog(outputTemplate);
        services.AddRabbitMq(configuration);

        services.AddSingleton<EmailConfiguration>(provider => new EmailConfiguration
        {
            SmtpServer = configuration["Email:SmtpSettings:SmtpServer"]!,
            SmtpPort = int.Parse(configuration["Email:SmtpSettings:SmtpPort"]!),
            SmtpUsername = configuration["Email:SmtpSettings:SmtpUsername"]!,
            SmtpPassword = configuration["Email:SmtpSettings:SmtpPassword"]!,
            SmtpFrom = configuration["Email:SmtpSettings:SmtpFrom"]!
        });
    })
    .ConfigureRabbitMqConsumers(collection => { collection.AddHostedService<ExampleConsumer>(); })
    .UseSerilog()
    .Build();

host.Run();