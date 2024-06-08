using CrackHash.Worker.Api.Consumers;
using CrackHash.Worker.Application.Services.Messaging;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<WorkerTaskResponsePublisher>();

builder.Services.AddMassTransit(busConfig =>
{
    busConfig.AddConsumer<WorkerTaskConsumer>()
        .Endpoint(e => e.Name = "WorkerQueue");
    
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
