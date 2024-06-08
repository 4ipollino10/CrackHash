using System.Text.Json.Serialization;
using CrackHash.Manager.Api.Consumers;
using CrackHash.Manager.Application.Jobs;
using CrackHash.Manager.Application.Services;
using CrackHash.Manager.Application.Services.Messaging;
using CrackHash.Manager.Infrastructure;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<TaskManager>();

builder.Services.AddScoped<WorkerTaskPublisher>();

builder.Services.AddMassTransit(busConfig =>
{   
    busConfig.AddConsumer<WorkerTaskResultConsumer>()
        .Endpoint(e => e.Name = "ManagerQueue");
    
    busConfig.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.PrefetchCount = 1;
        cfg.Durable = true;
        cfg.PurgeOnStartup = false;

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddMvc().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});



builder.Services.AddDbContext<ApplicationDbContext>(
    opt =>
    {
        opt.UseMongoDB("mongodb://mongo_primary:27017/", "test");
    });

builder.Services.AddQuartz(options =>
{
    var taskManagerJobKey = JobKey.Create(nameof(TaskManagerJob));
    options
        .AddJob<TaskManagerJob>(taskManagerJobKey)
        .AddTrigger(
            trigger => trigger
                .ForJob(taskManagerJobKey)
                .WithSimpleSchedule(
                    schedule => schedule
                        .WithIntervalInSeconds(1)
                        .RepeatForever()
                )
        );
    
    var taskProcessJobKey = JobKey.Create(nameof(TaskProcessJob));
    options
        .AddJob<TaskProcessJob>(taskProcessJobKey)
        .AddTrigger(
            trigger => trigger
                .ForJob(taskProcessJobKey)
                .WithSimpleSchedule(
                    schedule => schedule
                        .WithIntervalInSeconds(1)
                        .RepeatForever()
                )
        );
});
builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();