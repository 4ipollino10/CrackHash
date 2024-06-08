using CrackHash.Manager.Application.Models;
using CrackHash.Manager.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CrackHash.Manager.Api.Controllers;

[ApiController, Route("task-manager/api")]
public sealed class TaskManagerController
{
    private readonly TaskManager _taskManager;

    public TaskManagerController(TaskManager taskManager)
    {
        _taskManager = taskManager;
    }

    [HttpPost, Route("crack")]
    public async Task<Guid> CrackHash(CrackHashRequestModel model)
    {
        return await _taskManager.AddNewTask(model);
    }

    [HttpGet, Route("task-state/{requestId}")]
    public async Task<TaskState> GetTaskState([FromRoute] Guid requestId)
    {
        return await _taskManager.GetTaskState(requestId);
    }
}