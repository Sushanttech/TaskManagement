using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        [Required] public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public bool Completed { get; set; }
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        public int? AssignedToUserId { get; set; }
        public User? AssignedToUser { get; set; }
    }
}
