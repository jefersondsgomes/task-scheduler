using Quartz;
using WorkerService.Services.Interfaces;

namespace WorkerService.BackgroundTasks;

public class QuartzTask(
    ISchedulerFactory schedulerFactory,
    IQuartzTaskService quartzTaskService,
    ILogger<QuartzTask> logger) : BackgroundService
{
    private const int FREQUENCY_IN_SECONDS = 5;

    private readonly ISchedulerFactory _schedulerFactory = schedulerFactory;
    private readonly IQuartzTaskService _quartzTaskService = quartzTaskService;
    private readonly ILogger<QuartzTask> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var job = JobBuilder
            .Create<IQuartzTaskService>()
            .WithIdentity(_quartzTaskService.Id, nameof(IQuartzTaskService))
            .Build();

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
