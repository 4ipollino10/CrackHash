using CrackHash.Manager.Application.Services;
using CrackHash.Manager.Core.Entities;
using Quartz;

namespace CrackHash.Manager.Application.Jobs;

public class TaskManagerJob : IJob
{
    private readonly TaskManager _taskManager;

    public TaskManagerJob(TaskManager taskManager)
    {
        _taskManager = taskManager;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        var plannedTaskRequests = await _taskManager.Query<TaskRequest, List<TaskRequest>>(items =>
        {
            return Task.FromResult(items.Where(x => x.Status == TaskRequestStatus.Planned).ToList());
        });

        if (plannedTaskRequests.Count == 0)
        {
            return;
        }
        
        foreach(var plannedTaskRequest in plannedTaskRequests)
        {
            plannedTaskRequest.DivideTask();
        }

        await _taskManager.SaveChangesAsync();
    }
}