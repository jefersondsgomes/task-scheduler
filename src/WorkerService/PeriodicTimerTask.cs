using WorkerService.Services.Interfaces;

namespace WorkerService;

public class PeriodicTimerTask(
    IServiceProvider serviceProvider,
    ILogger<PeriodicTimerTask> logger) : BackgroundService
{
    private readonly ITaskService _taskService = serviceProvider
        .CreateScope()
        .ServiceProvider
        .GetRequiredService<ITaskService>();

    private readonly ILogger<PeriodicTimerTask> _logger = logger;

    private static readonly TimeSpan _frequency = TimeSpan.FromSeconds(5);
    private readonly PeriodicTimer _periodicTimer = new(_frequency);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.Clear();

        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        while (await _periodicTimer.WaitForNextTickAsync(stoppingToken))
        {
            // The PeriodicTimer Tick will be delayed until TaskService.ExecuteAsync finished. 
            // With this configuration, there is no multiple executions at same time.
            await _taskService.ExecuteAsync(stoppingToken);

            // This approach will execute regardless of the PeriodicTimer Tick because is not being awaited.
            // In this case, we must handle te logic of "IsRunning" inside de main service to avoid unnecessary concurrency.
            //_ = _taskService.ExecuteAsync(stoppingToken);
        }

        Environment.Exit(0);
    }
}