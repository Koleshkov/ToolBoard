using Microsoft.EntityFrameworkCore;
using ToolBoard.Data.Entities;
using ToolBoard.Data.Entities.Base;

namespace ToolBoard.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>(e =>
            {
                e.Property(p => p.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(p => p.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<ToolSticker>(e =>
            {
                e.Property(p => p.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(p => p.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<SurfaceSticker>(e =>
            {
                e.Property(p => p.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(p => p.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Board>(e =>
            {
                e.Property(p => p.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.Property(p => p.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<ToolSticker> ToolStickers { get; set; }
        public DbSet<SurfaceSticker> SurfaceStickers { get; set; }
        public DbSet<Board> Boards { get; set; }
    }
}
