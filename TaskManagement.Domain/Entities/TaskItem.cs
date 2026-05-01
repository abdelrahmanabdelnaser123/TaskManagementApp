using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities
{


    public class TaskItem : BaseEntity
    {
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public TaskStatusEnum Status { get; set; } = TaskStatusEnum.Pending;

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public int UserId { get; set; }

        public ApplicationUser User { get; set; } 

      
    }
}
