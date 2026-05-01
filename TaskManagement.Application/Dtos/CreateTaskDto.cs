using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Dtos
{
    public class CreateTaskDto
    {
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    }
}
