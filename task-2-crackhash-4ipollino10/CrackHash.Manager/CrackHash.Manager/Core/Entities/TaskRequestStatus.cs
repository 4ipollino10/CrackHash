namespace CrackHash.Manager.Core.Entities;

public enum TaskRequestStatus
{
    /// <summary>
    /// Задача запланирована, но не готова быть передана воркерам в работу
    /// </summary>
    Planned = 0,
    
    /// <summary>
    /// Задача готова быть передана воркерам
    /// </summary>
    ReadyToProcess = 1,
    
    /// <summary>
    /// Задача в работе у воркеров
    /// </summary>
    InProgress = 2,
    
    /// <summary>
    /// Задача завершена
    /// </summary>
    Completed = 3,
    
    /// <summary>
    /// Задача завершилась ошибкой
    /// </summary>
    Error = 4,
}