using CrackHash.Manager.Core.Entities;
using JetBrains.Annotations;

namespace CrackHash.Manager.Application.Models;

[PublicAPI]
public class TaskState
{
    /// <summary>
    /// Идентификатор запроса пользователя
    /// </summary>
    public Guid RequestId { get; set; }

    /// <summary>
    /// Хеш пользователя
    /// </summary>
    public string Hash { get; set; } = null!;

    /// <summary>
    /// Максимальная длинна слова
    /// </summary>
    public int MaxLength { get; set; }
    
    /// <summary>
    /// Статус задачи
    /// </summary>
    public TaskRequestStatus RequestStatus { get; set; }

    /// <summary>
    /// Найденные ответы
    /// </summary>
    public List<string>? Answer { get; set; }

    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string? ErrorMessage { get; set; }
}