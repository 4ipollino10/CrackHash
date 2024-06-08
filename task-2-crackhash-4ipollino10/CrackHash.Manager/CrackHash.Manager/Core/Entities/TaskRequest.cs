using CrackHash.Common.MessagingContract;
using CrackHash.Manager.Application.Models;
using JetBrains.Annotations;

namespace CrackHash.Manager.Core.Entities;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class TaskRequest
{
    private const string IncorrectHashErrorText = "Некорректный хэш";
    
    private TaskRequest()
    {
        
    }
    
    public TaskRequest(CrackHashRequestModel model)
    {
        Status = TaskRequestStatus.Planned;
        Hash = model.Hash;
        MaxLength = model.MaxLength;
    }
    
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; private set; }
    
    /// <summary>
    /// Статус задачи
    /// </summary>
    public TaskRequestStatus Status { get; private set; }
    
    /// <summary>
    /// Результат выполнения задачи
    /// </summary>
    public List<string>? Result { get; private set; }

    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string? ErrorMessage { get; private set; }

    /// <summary>
    /// Хеш слова
    /// </summary>
    public string Hash { get; private set; } = null!;

    /// <summary>
    /// Максимальная длина слова
    /// </summary>
    public int MaxLength { get; private set; }

    /// <summary>
    /// Подзадачи для воркеров
    /// </summary>
    public List<WorkerTask> WorkerTasks { get; private set; } = new();

    public void DivideTask()
    {
        WorkerTasks = new List<WorkerTask>();

        for (var i = 0; i < 10; ++i)
        {
            WorkerTasks.Add(new WorkerTask(this, i));
        }

        Status = TaskRequestStatus.ReadyToProcess;
    }

    public void InProcess()
    {
        Status = TaskRequestStatus.InProgress;
    }

    public void CompleteWorkerTask(WorkerTaskResultMessage message)
    {
        var workerTaskToComplete = WorkerTasks.FirstOrDefault(x => x.Id == message.TaskId);

        if (workerTaskToComplete is null)
        {
            //throw
            return;
        }

        workerTaskToComplete.Complete(message);

        if (workerTaskToComplete.ErrorMessage == IncorrectHashErrorText)
        {
            ErrorMessage = IncorrectHashErrorText;
            Status = TaskRequestStatus.Completed;
            return;
        }
        
        var isComplete = true;
        foreach (var workerTask in WorkerTasks)
        {
            if (workerTask.Status is WorkerTaskStatus.Completed)
            {
                if (workerTask.Result is null) continue;
                
                Result = workerTask.Result;
                break;
            }

            isComplete = false;
        }

        if (!isComplete) return;
        
        Status = TaskRequestStatus.Completed;
        if (Result is null)
        {
            ErrorMessage = "Слово не было найдено";
        }
    }
}