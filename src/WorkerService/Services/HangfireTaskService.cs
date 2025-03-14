using WorkerService.Services.Interfaces;

namespace WorkerService.Services;

public class HangfireTaskService(ILogger<TaskService> logger) : TaskService(logger), IHangfireTaskService
{
    public override string Name => "Hangfire Task Service";
}
