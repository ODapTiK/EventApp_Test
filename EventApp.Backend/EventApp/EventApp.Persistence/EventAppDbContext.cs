using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EventApp
{
    public class EventAppDbContext : DbContext, IEventAppDbContext
    {

        public EventAppDbContext(DbContextOptions<EventAppDbContext> options) : base(options) { }

        public DbSet<EventModel> Events { get; set; }
        public DbSet<ParticipantModel> Participants { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        public DbSet<AdminModel> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            modelBuilder.ApplyConfiguration(new ParticipantConfiguration());
            modelBuilder.ApplyConfiguration(new AdminConfiguration());

            modelBuilder.Entity<EventParticipant>()
                .HasKey(ep => new { ep.EventId, ep.ParticipantId });

            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.Event)
                .WithMany(e => e.Participants)
                .HasForeignKey(ep => ep.EventId);

            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.Participant)
                .WithMany(p => p.Events)
                .HasForeignKey(ep => ep.ParticipantId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
