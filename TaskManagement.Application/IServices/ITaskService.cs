using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagement.Application.Dtos;
using TaskManagement.Application.Response;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.IServices
{
    public interface ITaskService
    {
        Task<Response<int>> CreateTask(CreateTaskDto dto);
        Task<Response<TaskDto>> GetTaskById(int id);
        Task<Response<List<TaskDto>>> GetMyTasks();
        Task<Response<bool>> UpdateTaskStatus(int taskId, TaskStatusEnum status);
        Task<Response<List<TaskDto>>> GetAllTasks();
    }
}
