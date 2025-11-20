using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.Models
{
    public class Role
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; } = default!;
        public ICollection<User>? Users { get; set; }
    }
}
