using Quartz;
using WorkerService.Services.Interfaces;

namespace WorkerService.BackgroundTasks;

public class QuartzTask(
    ISchedulerFactory schedulerFactory,
    ILogger<QuartzTask> logger) : BackgroundService
{
    private const int FREQUENCY_IN_SECONDS = 5;

    private readonly ISchedulerFactory _schedulerFactory = schedulerFactory;
    private readonly ILogger<QuartzTask> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Create job configuration, the type must implement IJob interface
        var job = JobBuilder
            .Create<IQuartzTaskService>()
            .WithIdentity("QuartzTask", nameof(IQuartzTaskService))
            .Build();

        // Create trigger behavior
        var trigger = TriggerBuilder
            .Create()
            .StartNow()
            .WithSimpleSchedule(builder =>
            {
                builder.WithIntervalInSeconds(FREQUENCY_IN_SECONDS);
                builder.RepeatForever();
            })
            .Build();

        var scheduler = await _schedulerFactory.GetScheduler(stoppingToken);

        // Schedule the job and call "Execute" method when triggered
        await scheduler.ScheduleJob(job, trigger, stoppingToken);
    }
}
