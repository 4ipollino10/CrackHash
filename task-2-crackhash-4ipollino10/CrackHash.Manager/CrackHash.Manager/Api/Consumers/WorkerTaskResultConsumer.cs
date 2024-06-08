using CrackHash.Common.MessagingContract;
using CrackHash.Manager.Application.Services;
using MassTransit;

namespace CrackHash.Manager.Api.Consumers;

public class WorkerTaskResultConsumer : IConsumer<WorkerTaskResultMessage>
{
    private readonly TaskManager _taskManager;

    public WorkerTaskResultConsumer(TaskManager taskManager)
    {
        _taskManager = taskManager;
    }

    public async Task Consume(ConsumeContext<WorkerTaskResultMessage> context)
    {
        await _taskManager.CompleteWorkerTask(context.Message);
    }
}