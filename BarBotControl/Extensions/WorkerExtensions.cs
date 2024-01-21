using BarBotControl.Comms.Models;
using BarBotControl.Worker.Services;
using BarBotControl.Comms.Worker;

namespace BarBotControl.Extensions;

public static class WorkerExtensions
{
    public static WebApplicationBuilder AddWorker(this WebApplicationBuilder builder)
    {
        var workerConfig = builder.Configuration.GetSection("WorkerConfig").Get<WorkerConfig>();
        CommsWorker.Config = workerConfig;
        var schedulerLimmit = (int)builder.Configuration.GetValue(typeof(int), "SchedulerConfig:QueueLimmit");
        schedulerLimmit = schedulerLimmit == 0 ? 10 : schedulerLimmit;
        builder.Services.AddSingleton(new ScheduledWorkerService<RequestModel, ResponseModel>(schedulerLimmit, CommsWorker.Run));

        return builder;
    }
}
