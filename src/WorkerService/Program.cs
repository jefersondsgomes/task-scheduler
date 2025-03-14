using WorkerService.BackgroundTasks;
using WorkerService.Extensions;
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
                .AddSingleton<IPeriodicTimerTaskService, PeriodicTimerTaskService>()
                .AddSingleton<IHangfireTaskService, HangfireTaskService>()
                .AddSingleton<IQuartzTaskService, QuartzTaskService>()
                .ConfigureHangfire()
                .ConfigureQuartz();

            builder.Services
                .AddHostedService<PeriodicTimerTask>()
                .AddHostedService<HangfireTask>()
                .AddHostedService<QuartzTask>();

            var host = builder.Build();

            host.Run();
        }
    }
}
