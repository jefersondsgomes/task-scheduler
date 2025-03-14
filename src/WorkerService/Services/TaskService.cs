using WorkerService.Services.Interfaces;

namespace WorkerService.Services;

public abstract class TaskService(ILogger<TaskService> logger) : ITaskService
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public abstract string Name { get; }
    public bool IsRunning { get; private set; }


    private readonly ILogger<TaskService> _logger = logger;

    private static readonly Random _random = new();

    public async virtual Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting {Name} - '{Id}' task execution", Name, Id);

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
                throw new Exception($"Task {Name} - '{Id}' failed during execution");
            }

            await Task.Delay(taskDurationInSeconds, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);

            // Throw the exception will require handling configurations for Hangfire and Quartz.
            // throw;

            return;
        }
        finally
        {
            IsRunning = false;
        }

        _logger.LogInformation("Task {Name} - '{Id}' executed successfully", Name, Id);
    }
}
