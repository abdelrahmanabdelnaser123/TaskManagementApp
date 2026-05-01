using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Dtos
{
    public class TaskDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public TaskStatusEnum Status { get; set; }

        public TaskPriority Priority { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
