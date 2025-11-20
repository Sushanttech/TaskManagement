using System.Linq;
using TaskManager.Api.Models;

namespace TaskManager.Api.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            if (!db.Roles.Any())
            {
                db.Roles.AddRange(new Role { Name = "Admin" }, new Role { Name = "User" });
                db.SaveChanges();
            }

            if (!db.Users.Any())
            {
                var adminRole = db.Roles.First(r => r.Name == "Admin");
                var userRole = db.Roles.First(r => r.Name == "User");

                db.Users.Add(new User
                {
                    Username = "admin",
                    FullName = "Administrator",
                    RoleId = adminRole.Id,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!")
                });

                db.Users.Add(new User
                {
                    Username = "user",
                    FullName = "Demo User",
                    RoleId = userRole.Id,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!")
                });

                db.SaveChanges();
            }

            if (!db.Projects.Any())
            {
                var p = new Project { Title = "Initial Project", Description = "Seeded project" };
                db.Projects.Add(p);
                db.SaveChanges();

                db.Tasks.AddRange(
                    new TaskItem { Title = "Seed Task 1", Description = "First task", ProjectId = p.Id, Completed = false },
                    new TaskItem { Title = "Seed Task 2", Description = "Second task", ProjectId = p.Id, Completed = true }
                );
                db.SaveChanges();
            }
        }
    }
}
