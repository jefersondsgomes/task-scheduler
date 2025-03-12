using WorkerService.Services;
using WorkerService.Services.Interfaces;

namespace WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services
                .AddTransient<ITaskService, TaskService>()
                .AddHostedService<PeriodicTimerTask>();

            var host = builder.Build();

            host.Run();
        }
    }
}