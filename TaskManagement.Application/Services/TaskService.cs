using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Application.Dtos;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.IServices;
using TaskManagement.Application.Response;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthServices _authService;
        private readonly ITaskQueue _queue;
        private readonly IDistributedCache _cache;

        public TaskService(
            IUnitOfWork unitOfWork,
            IAuthServices authService,
            ITaskQueue queue,
            IDistributedCache cache)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _queue = queue;
            _cache = cache;
        }

        // ================= CREATE TASK =================
        public async Task<Response<int>> CreateTask(CreateTaskDto dto)
        {
            var userId = await _authService.GetCurrentUserId();

            if (userId is null)
            {
                return new Response<int>
                {
                    Status = ResponseStatus.Unauthorized,
                    Message = "User not authenticated"
                };
            }

            var uid = userId.Value;

            var repo = _unitOfWork.Repository<TaskItem>();

            var today = DateTime.UtcNow.Date;

            // 
            bool exists = await repo.IsExisit(x =>
                x.Title == dto.Title &&
                x.UserId == uid &&
                x.CreatedAt >= today &&
                x.CreatedAt < today.AddDays(1)
            );

            if (exists)
            {
                return new Response<int>
                {
                    Status = ResponseStatus.BadRequest,
                    Message = "Task with same title already exists today"
                };
            }

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                Status = TaskStatusEnum.Pending,
                UserId = uid,
                CreatedAt = DateTime.UtcNow
            };

            repo.Add(task);
            await _unitOfWork.SaveAsync();

            // Background queue
            _queue.Enqueue(task.Id);

            return new Response<int>
            {
                Data = task.Id,
                Status = ResponseStatus.Success,
                Message = "Task created successfully"
            };
        }

        // ================= GET TASK BY ID =================




        public async Task<Response<TaskDto>> GetTaskById(int id)
        {
            var cacheKey = $"task:{id}";

            //  Check Redis first
            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                var cachedTask = JsonSerializer.Deserialize<TaskDto>(cachedData);

                return new Response<TaskDto>
                {
                    Data = cachedTask,
                    Status = ResponseStatus.Success,
                    Message = "From Cache"
                };
            }

            //  Get from DB
            var repo = _unitOfWork.Repository<TaskItem>();

            var task = await repo.GetByAsync(x => x.Id == id && !x.IsDeleted);

            if (task == null)
            {
                return new Response<TaskDto>
                {
                    Status = ResponseStatus.NotFound,
                    Message = "Task not found"
                };
            }

            var result = new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                CreatedAt = task.CreatedAt
            };

            //  Save to Redis
            await _cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(result),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });

            return new Response<TaskDto>
            {
                Data = result,
                Status = ResponseStatus.Success,
                Message = "From Database"
            };
        }

        // ================= GET MY TASKS =================
        public async Task<Response<List<TaskDto>>> GetMyTasks()
        {
            var userId = await _authService.GetCurrentUserId();

            if (userId is null)
            {
                return new Response<List<TaskDto>>
                {
                    Status = ResponseStatus.Unauthorized,
                    Message = "User not authenticated"
                };
            }

            var uid = userId.Value;

            var repo = _unitOfWork.Repository<TaskItem>();

            var tasks = await repo.GetAllByAsync(x =>
                x.UserId == uid &&
                !x.IsDeleted
            );

            var result = tasks
                .OrderBy(x => x.Priority)
                .ThenBy(x => x.CreatedAt)
                .Select(x => new TaskDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Status = x.Status,
                    Priority = x.Priority,
                    CreatedAt = x.CreatedAt
                })
                .ToList();

            return new Response<List<TaskDto>>
            {
                Data = result,
                Status = ResponseStatus.Success
            };
        }

        // ================= UPDATE TASK STATUS =================
        public async Task<Response<bool>> UpdateTaskStatus(int taskId, TaskStatusEnum status)
        {
            var userId = await _authService.GetCurrentUserId();

            var repo = _unitOfWork.Repository<TaskItem>();

            var task = await repo.GetByAsync(x =>
                x.Id == taskId &&
                x.UserId == userId &&
                !x.IsDeleted
            );

            if (task == null)
            {
                return new Response<bool>
                {
                    Status = ResponseStatus.NotFound,
                    Message = "Task not found"
                };
            }

            task.Status = status;
            task.UpdatedAt = DateTime.UtcNow;

            repo.Update(task);
            await _unitOfWork.SaveAsync();

            //  REMOVE CACHE after update
            await _cache.RemoveAsync($"task:{taskId}");

            return new Response<bool>
            {
                Data = true,
                Status = ResponseStatus.Success,
                Message = "Task updated successfully"
            };
        }
        public async Task<Response<List<TaskDto>>> GetAllTasks()
        {
            var repo = _unitOfWork.Repository<TaskItem>();

            var tasks = await repo.GetAllByAsync(x => !x.IsDeleted);

            var result = tasks
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new TaskDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Status = x.Status,
                    Priority = x.Priority,
                    CreatedAt = x.CreatedAt
                })
                .ToList();

            return new Response<List<TaskDto>>
            {
                Data = result,
                Status = ResponseStatus.Success
            };
        }
        private bool IsValidStatusTransition(TaskStatusEnum current, TaskStatusEnum next)
        {
            return (current, next) switch
            {
                (TaskStatusEnum.Pending, TaskStatusEnum.InProgress) => true,
                (TaskStatusEnum.InProgress, TaskStatusEnum.Done) => true,

                // allow same state (optional)
                (var a, var b) when a == b => true,

                _ => false
            };
        }
    }
}