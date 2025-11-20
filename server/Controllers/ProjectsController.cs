using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Data;
using TaskManager.Api.Models;

namespace TaskManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ProjectsController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _db.Projects.Include(p => p.Tasks).ToListAsync();
            return Ok(projects);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Project project)
        {
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _db.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == id);
            if (project == null) return NotFound();
            return Ok(project);
        }

        [HttpGet("reports/tasks-per-project")]
        public async Task<IActionResult> TasksPerProject()
        {
            var q = await _db.Projects
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    TotalTasks = p.Tasks.Count(),
                    Completed = p.Tasks.Count(t => t.Completed),
                    Pending = p.Tasks.Count(t => !t.Completed)
                }).ToListAsync();

            return Ok(q);
        }
    }
}
