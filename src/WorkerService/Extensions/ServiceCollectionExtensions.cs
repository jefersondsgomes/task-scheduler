using Hangfire;
using Hangfire.MemoryStorage;
using Quartz;

namespace WorkerService.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection ConfigureHangfire(this IServiceCollection services)
    {
        // Add persistency configuration to store job data in memory
        services.AddHangfire(config =>
        {
            config.UseSimpleAssemblyNameTypeSerializer();
            config.UseRecommendedSerializerSettings();
            config.UseMemoryStorage();
        });

        // Register Hangfire server that will query storage (in this case, memory) and execute the jobs.        
        services.AddHangfireServer(options =>
        {
            // Check registered items in database every 10 seconds
            options.SchedulePollingInterval = TimeSpan.FromSeconds(10);
        });

        return services;
    }

    internal static IServiceCollection ConfigureQuartz(this IServiceCollection services)
    {
        services.AddQuartz(quartzConfig =>
        {
            quartzConfig.UseDefaultThreadPool(config => config.MaxConcurrency = 1);
            quartzConfig.MisfireThreshold = TimeSpan.FromSeconds(10);
            quartzConfig.UseInMemoryStore();
        });

        services.AddQuartzHostedService(quartz =>
        {
            quartz.AwaitApplicationStarted = true;
            quartz.WaitForJobsToComplete = true;
        });

        return services;
    }
}
