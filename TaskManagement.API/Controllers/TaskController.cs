using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Dtos;
using TaskManagement.Application.IServices;
using TaskManagement.Application.Response;
using TaskManagement.Domain.Enums;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController(ITaskService taskService) : ControllerBase
    {
        private readonly ITaskService _taskService = taskService;
        [HttpPost]
        public async Task<Response<int>> CreateTask([FromBody] CreateTaskDto dto)
        {
            return await _taskService.CreateTask(dto);
        }
        [HttpGet("{id}")]
        public async Task<Response<TaskDto>> GetTaskById(int id)
        {
            return await _taskService.GetTaskById(id);
        }
        [HttpGet("my-tasks")]
        public async Task<Response<List<TaskDto>>> GetMyTasks()
        {
            return await _taskService.GetMyTasks();
        }
        [HttpPut("status")]
        public async Task<Response<bool>> UpdateTaskStatus([FromBody] UpdateTaskStatusDto dto)
        {
            return await _taskService.UpdateTaskStatus(dto.TaskId, dto.Status);
        }
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<Response<List<TaskDto>>> GetAllTasks()
        {
            return await _taskService.GetAllTasks();
        }
     
    }
}
    
