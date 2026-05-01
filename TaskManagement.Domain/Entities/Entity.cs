using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Entities
{
    public class Entity<T>
    {
        [Key]
        public T Id { get; protected set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; } = false;

        public Entity(T id)
        {
            Id = id;
        }

        public Entity() { }
    }
    public class BaseEntity : Entity<int>
    {

    }
}
