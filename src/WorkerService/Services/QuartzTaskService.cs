using Quartz;
using WorkerService.Services.Interfaces;

namespace WorkerService.Services;

public class QuartzTaskService(ILogger<TaskService> logger) : TaskService(logger), IQuartzTaskService
{
    public override string Name => "Quartz Task Service";

    public async Task Execute(IJobExecutionContext context)
    {
        await base.ExecuteAsync(default);
    }
}
