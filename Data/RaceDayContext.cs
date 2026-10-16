
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Models;

namespace RaceDayAPI.Data
{
    public class RaceDayContext : DbContext
    {
        public RaceDayContext(DbContextOptions<RaceDayContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Events");

                entity.HasKey(e => e.EventID);

                entity.Property(e => e.EventID)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.EventName)
                    .IsRequired();

                entity.Property(e => e.EventDistance)
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.EventType)
                    .IsRequired();
            });
        }
    }
}
