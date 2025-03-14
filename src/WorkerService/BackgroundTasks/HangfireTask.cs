using Hangfire;
using WorkerService.Services.Interfaces;

namespace WorkerService.BackgroundTasks;

public class HangfireTask(
    IHangfireTaskService hangfireTaskService,
    IRecurringJobManager recurringJobManager,
    ILogger<HangfireTask> logger) : BackgroundService
{
    const string JOB_FREQUENCY = "*/5 * * * * *";

    private readonly IHangfireTaskService _hangfireTaskService = hangfireTaskService;
    private readonly IRecurringJobManager _recurringJobManager = recurringJobManager;
    private readonly ILogger<HangfireTask> _logger = logger;

    private static readonly RecurringJobOptions _options = new()
    {
        // Determine the behavior for mis fires
        MisfireHandling = MisfireHandlingMode.Relaxed
    };

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        // Register the job to be processed by the Hangfire server
        _recurringJobManager.AddOrUpdate<IHangfireTaskService>(
            _hangfireTaskService.Id,
            _ => _hangfireTaskService.ExecuteAsync(stoppingToken),
            JOB_FREQUENCY,
            _options);

        return Task.CompletedTask;
    }
}
