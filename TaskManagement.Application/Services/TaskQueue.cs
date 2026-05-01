using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.Application.Services
{
    public class TaskQueue : ITaskQueue
    {
        private readonly ConcurrentQueue<int> _queue = new();

        public void Enqueue(int taskId)
        {
            _queue.Enqueue(taskId);
        }

        public int Dequeue()
        {
            _queue.TryDequeue(out var taskId);
            return taskId;
        }
    }
}
