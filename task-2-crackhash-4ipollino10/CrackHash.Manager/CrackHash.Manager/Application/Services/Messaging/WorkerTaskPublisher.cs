using CrackHash.Common.MessagingContract;
using CrackHash.Manager.Core.Entities;
using CrackHash.Manager.Infrastructure;
using MassTransit;

namespace CrackHash.Manager.Application.Services.Messaging;

public class WorkerTaskPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    private readonly ApplicationDbContext _applicationDbContext;

    public WorkerTaskPublisher(IPublishEndpoint publishEndpoint, ApplicationDbContext applicationDbContext)
    {
        _publishEndpoint = publishEndpoint;
        _applicationDbContext = applicationDbContext;
    }

    public async Task SendTaskToWorkers(TaskRequest taskRequest)
    {
        foreach (var workerTask in taskRequest.WorkerTasks)
        {
            if (workerTask.Status is not WorkerTaskStatus.ReadyToPlan)
            {
                return;
            }
            
            await _publishEndpoint.Publish(new WorkerTaskRequestMessage()
            {   
               MaxLength = taskRequest.MaxLength,
               Hash = taskRequest.Hash,
               TaskId = workerTask.Id,
               RequestId = taskRequest.Id,
               Skip = workerTask.Skip,
            });

            workerTask.Plan();
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}