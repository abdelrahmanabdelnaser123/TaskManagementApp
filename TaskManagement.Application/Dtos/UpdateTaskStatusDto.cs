using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Dtos
{
    public class UpdateTaskStatusDto
    {
        public int TaskId { get; set; }
        public TaskStatusEnum Status { get; set; }
    }
}
