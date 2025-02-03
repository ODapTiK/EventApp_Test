using Microsoft.EntityFrameworkCore;

namespace EventApp
{
    public interface IEventAppDbContext
    {
        public DbSet<EventModel> Events { get; set; }
        public DbSet<ParticipantModel> Participants { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        public DbSet<AdminModel> Admins { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
