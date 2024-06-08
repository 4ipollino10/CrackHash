namespace CrackHash.Manager.Core.Entities;

public enum WorkerTaskStatus
{
    /// <summary>
    /// Задача не отправлена в очередь
    /// </summary>
    ReadyToPlan = 1,
    
    /// <summary>
    /// Задача в очереди
    /// </summary>
    Planned = 2,
    
    /// <summary>
    /// Задача у воркера
    /// </summary>
    InProgress = 3,
    
    /// <summary>
    /// Задача завершена
    /// </summary>
    Completed = 4,
}