using CrackHash.Manager.Application.Services;
using CrackHash.Manager.Application.Services.Messaging;
using CrackHash.Manager.Core.Entities;
using Quartz;

namespace CrackHash.Manager.Application.Jobs;

public class TaskProcessJob : IJob
{
    private readonly TaskManager _taskManager;
    private readonly WorkerTaskPublisher _workerTaskPublisher;

    public TaskProcessJob(TaskManager taskManager, WorkerTaskPublisher workerTaskPublisher)
    {
        _taskManager = taskManager;
        _workerTaskPublisher = workerTaskPublisher;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        
        var readyToProcessRequests = await _taskManager.Query<TaskRequest, List<TaskRequest>>(items =>
        {
            return Task.FromResult(items.Where(x => x.Status == TaskRequestStatus.ReadyToProcess).ToList());
        });

        
        if (readyToProcessRequests.Count == 0)
        {
            return;
        }
        
        foreach (var readyToProcessRequest in readyToProcessRequests)
        {
            await _workerTaskPublisher.SendTaskToWorkers(readyToProcessRequest);
            
            readyToProcessRequest.InProcess();
            await _taskManager.SaveChangesAsync();
        }
    }
}