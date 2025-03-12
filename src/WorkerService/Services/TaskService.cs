using WorkerService.Services.Interfaces;

namespace WorkerService.Services;

public class TaskService(ILogger<TaskService> logger) : ITaskService
{
    private readonly ILogger<TaskService> _logger = logger;

    private static readonly Random _random = new();

    public string Id { get; } = Guid.NewGuid().ToString();
    public bool IsRunning { get; private set; }
    
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting '{Id}' task execution", Id);

        if (IsRunning)
        {
            _logger.LogInformation("The task is already running");

            return;
        }

        try
        {
            IsRunning = true;

            var taskDurationInSeconds = TimeSpan.FromSeconds(_random.Next(5, 20));

            _logger.LogInformation("Task duration in seconds: {TaskDurationInSeconds}", taskDurationInSeconds);

            var error = _random.Next(1, 5) > 3;

            if (error)
            {
                throw new Exception($"Task '{Id}' failed during execution");
            }

            await Task.Delay(taskDurationInSeconds, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);

            return;
        }
        finally
        {
            IsRunning = false;
        }

        _logger.LogInformation("Task executed successfully");
    }
}
