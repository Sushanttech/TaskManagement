using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Required] public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public ICollection<TaskItem>? Tasks { get; set; }
    }
}
