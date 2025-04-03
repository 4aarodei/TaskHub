using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskHub.Models;

namespace TaskHub.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<TeamModel> Teams { get; set; }
        public DbSet<TeamInvite> TeamInvites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Налаштування зовнішніх ключів
            modelBuilder.Entity<TaskModel>()
                .HasOne(t => t.AppUser)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskModel>()
                .HasOne(t => t.Team)
                .WithMany(team => team.Tasks)
                .HasForeignKey(t => t.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
