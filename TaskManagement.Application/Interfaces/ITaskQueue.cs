using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.Interfaces
{
    public interface ITaskQueue
    {
        void Enqueue(int taskId);
        int Dequeue();
    }
}
