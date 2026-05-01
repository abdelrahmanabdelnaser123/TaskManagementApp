using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Services
{
    public class TaskBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ITaskQueue _queue;

        public TaskBackgroundService(IServiceProvider serviceProvider, ITaskQueue queue)
        {
            _serviceProvider = serviceProvider;
            _queue = queue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var taskId = _queue.Dequeue();

                if (taskId != 0)
                {
                    using var scope = _serviceProvider.CreateScope();

                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var repo = unitOfWork.Repository<TaskItem>();

                    var task = await repo.GetByAsync(x => x.Id == taskId);

                    if (task != null)
                    {
                        // simulate processing
                        task.Status = TaskStatusEnum.InProgress;
                        await unitOfWork.SaveAsync();

                        await Task.Delay(2000, stoppingToken);

                        task.Status = TaskStatusEnum.Done;
                        task.UpdatedAt = DateTime.UtcNow;

                        repo.Update(task);
                        await unitOfWork.SaveAsync();
                    }
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
