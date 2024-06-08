using CrackHash.Worker.Application.Exceptions;
using CrackHash.Worker.Application.Services;
using CrackHash.Worker.Application.Services.Messaging;
using CrackHash.Common.MessagingContract;
using MassTransit;

namespace CrackHash.Worker.Api.Consumers;

public class WorkerTaskConsumer : IConsumer<WorkerTaskRequestMessage>
{
    private readonly WorkerTaskResponsePublisher _responsePublisher;
    
    public WorkerTaskConsumer(WorkerTaskResponsePublisher responsePublisher)
    {
        _responsePublisher = responsePublisher;
    }

    public async Task Consume(ConsumeContext<WorkerTaskRequestMessage> context)
    {
        List<string>? result = null;
        
        Console.WriteLine("Принял сообщение с идентификатором запроса: " + context.Message.RequestId + "и идентификатором задачи: " + context.Message.TaskId);
        
        try
        {
            result = WordCracker.CrackWord(context.Message);
        }
        catch (IncorrectHashException ex)
        {
            await _responsePublisher.SendCrackResult(new WorkerTaskResultMessage()
            {
                Result = null,
                ErrorMessage = ex.Message,
                TaskId = context.Message.TaskId,
                RequestId = context.Message.RequestId
            });
        }

        await _responsePublisher.SendCrackResult(new WorkerTaskResultMessage()
        {
            Result = result,
            ErrorMessage = result is null ? "Слово не было найдено" : null,
            TaskId = context.Message.TaskId,
            RequestId = context.Message.RequestId
        });
    }
}