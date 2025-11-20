using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Data;
using TaskManager.Api.Hubs;
using TaskManager.Api.Models;

namespace TaskManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IHubContext<TaskHub> _hub;
        public TasksController(AppDbContext db, IHubContext<TaskHub> hub) { _db = db; _hub = hub; }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _db.Tasks.Include(t => t.Project).ToListAsync());

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(TaskItem task)
        {
            _db.Tasks.Add(task);
            await _db.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("TaskUpdated", new { action = "created", task });

            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, TaskItem updated)
        {
            var task = await _db.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            task.Title = updated.Title;
            task.Description = updated.Description;
            task.Completed = updated.Completed;
            task.DueDate = updated.DueDate;
            task.AssignedToUserId = updated.AssignedToUserId;

            await _db.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("TaskUpdated", new { action = "updated", task });

            return Ok(task);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var t = await _db.Tasks.Include(x => x.Project).FirstOrDefaultAsync(x => x.Id == id);
            if (t == null) return NotFound();
            return Ok(t);
        }
    }
}
