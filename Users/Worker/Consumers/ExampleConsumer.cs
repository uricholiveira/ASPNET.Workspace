using System.Diagnostics;

namespace Worker.Consumers.Notification;

public class ExampleConsumer : BackgroundService
{
    private readonly ILogger<ExampleConsumer> _logger;

    public ExampleConsumer(ILogger<ExampleConsumer> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (Activity.Current is null)
        {
        }

        var activity = new Activity(nameof(ExampleConsumer));

        activity.Start();

        _logger.LogInformation("Hello World! From example consumer.");

        activity.Stop();
        
        while (!stoppingToken.IsCancellationRequested) await Task.Delay(1000, stoppingToken);
    }
}