using MassTransit;
using MassTransit.Configuration;
using Outbox.Publisher.Service;
using Quartz;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddQuartz(configurator =>
{
    JobKey jobKey = new("OrderOutboxPublisherJob");
    configurator.AddJob<OrderOutboxPublisherJob>(options => options.WithIdentity(jobKey));

    TriggerKey triggerKey = new("OrderOutboxPublisherJob");
    configurator.AddTrigger(options => options.ForJob(jobKey)
    .WithIdentity(triggerKey)
    .StartAt(DateTime.UtcNow)
    .WithSimpleSchedule(builder => builder
            .WithIntervalInSeconds(5)
            .RepeatForever()));
});
builder.Services.AddMassTransit(configurator =>
{
    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMQ"]);
    });
});
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
var host = builder.Build();
host.Run();
