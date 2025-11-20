using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required] public string Username { get; set; } = default!;
        [Required] public string PasswordHash { get; set; } = default!;
        public string? FullName { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
