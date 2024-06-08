using CrackHash.Common.MessagingContract;
using JetBrains.Annotations;

namespace CrackHash.Manager.Core.Entities;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class WorkerTask
{
    private WorkerTask()
    {
        
    }

    public WorkerTask(TaskRequest taskRequest, int skip)
    {
        Id = Guid.NewGuid();
        RequestId = taskRequest.Id;
        Skip = skip;
        Status = WorkerTaskStatus.ReadyToPlan;
    }
    
    /// <summary>
    /// Идентификатор задачи воркера
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Идентификатор запроса пользователя
    /// </summary>
    public Guid RequestId { get; private set; }

    /// <summary>
    /// Skip
    /// </summary>
    public int Skip { get; private set; }
    
    /// <summary>
    /// Результат задачи
    /// </summary>
    public List<string>? Result { get; private set; }

    /// <summary>
    /// Статус задачи воркера
    /// </summary>
    public WorkerTaskStatus Status { get; private set; }

    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string? ErrorMessage { get; private set; }

    /// <summary>
    /// Планирование задачи для воркера
    /// </summary>
    public void Plan()
    {
        Status = WorkerTaskStatus.Planned;
    }

    /// <summary>
    /// Завершение выполненной воркером задачи
    /// </summary>
    /// <param name="message"></param>
    public void Complete(WorkerTaskResultMessage message)
    {
        if (message.ErrorMessage is not null)
        {
            ErrorMessage = message.ErrorMessage;
            Result = null;
            Status = WorkerTaskStatus.Completed;
            return;
        }

        Result = message.Result;
        ErrorMessage = null;
        Status = WorkerTaskStatus.Completed;
    }
}