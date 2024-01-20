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
        builder.Services.AddSingleton(new ScheduledWorkerService<RequestModel, ResponseModel>(CommsWorker.Run));

        return builder;
    }
}
