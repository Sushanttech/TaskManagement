using System;

namespace TaskManager.Api.Models
{
    public class RequestResponseLog
    {
        public int Id { get; set; }
        public string Method { get; set; } = default!;
        public string Path { get; set; } = default!;
        public string? RequestBody { get; set; }
        public string? ResponseBody { get; set; }
        public int ResponseStatusCode { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? Username { get; set; }
    }
}
