namespace WorkerService.Services.Interfaces;

public interface ITaskService
{
    string Id { get; }
    string Name { get; }
    bool IsRunning { get; }
    /// <summary>
    /// Simulates a task execution
    /// </summary>   
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}
