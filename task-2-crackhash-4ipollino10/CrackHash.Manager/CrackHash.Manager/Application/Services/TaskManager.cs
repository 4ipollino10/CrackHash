using CrackHash.Common.MessagingContract;
using CrackHash.Manager.Application.Models;
using CrackHash.Manager.Core.Entities;
using CrackHash.Manager.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CrackHash.Manager.Application.Services;

/// <summary>
/// Менеджер задач
/// </summary>
public sealed class TaskManager
{
    private readonly ApplicationDbContext _applicationDbContext;

    public TaskManager(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    /// <summary>
    /// Добавление новой задачи
    /// </summary>
    /// <param name="model"></param>
    public async Task<Guid> AddNewTask(CrackHashRequestModel model)
    {
        var newTask = _applicationDbContext.Add(new TaskRequest(model));

        await _applicationDbContext.SaveChangesAsync();
        
        return newTask.Entity.Id;
    }

    /// <summary>
    /// Получение состояние задачи
    /// </summary>
    /// <param name="requestId"></param>
    public async Task<TaskState> GetTaskState(Guid requestId)
    {
        var taskRequest = await _applicationDbContext.Set<TaskRequest>().FirstOrDefaultAsync(x => x.Id == requestId);

        if (taskRequest is null)
        {
            //throw
            return new TaskState();
        }
        
        return new TaskState()
        {
            RequestId = taskRequest.Id,
            Answer = taskRequest.Result ?? null,
            MaxLength = taskRequest.MaxLength,
            Hash = taskRequest.Hash,
            RequestStatus = taskRequest.Status,
            ErrorMessage = taskRequest.ErrorMessage,
        };
    }

    /// <summary>
    /// Завершить выполненную воркером задачу
    /// </summary>
    /// <param name="message"></param>
    public async Task CompleteWorkerTask(WorkerTaskResultMessage message)
    {
        var taskRequest = await _applicationDbContext.TaskRequests.FirstOrDefaultAsync(x => x.Id == message.RequestId);

        if (taskRequest is null)
        {
            //throw
            return;
        }
        
        if (taskRequest.Status is TaskRequestStatus.Completed)
        {
            return;
        }

        taskRequest.CompleteWorkerTask(message);
        
        await _applicationDbContext.SaveChangesAsync();
    }
    
    /// <summary>
    /// Запрос к базе
    /// </summary>
    /// <param name="selector"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public Task<TResult> Query<TEntity, TResult>(Func<IQueryable<TEntity>, Task<TResult>> selector) where TEntity : class
    {
        var query =
            _applicationDbContext.Set<TEntity>();
        return selector(query);
    }

    /// <summary>
    /// Сохранение состояния базы
    /// </summary>
    public async Task SaveChangesAsync()
    {
        await _applicationDbContext.SaveChangesAsync();
    } 
}