using WorkerService.Services.Interfaces;

namespace WorkerService.Services;

public class PeriodicTimerTaskService(ILogger<TaskService> logger) : TaskService(logger), IPeriodicTimerTaskService
{
    public override string Name => "Periodic Timer Task";
}
