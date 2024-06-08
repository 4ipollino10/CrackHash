using CrackHash.Common.MessagingContract;
using MassTransit;

namespace CrackHash.Worker.Application.Services.Messaging;

public class WorkerTaskResponsePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public WorkerTaskResponsePublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task SendCrackResult(WorkerTaskResultMessage workerTaskResultMessage)
    {
        await _publishEndpoint.Publish(workerTaskResultMessage);
    }
}