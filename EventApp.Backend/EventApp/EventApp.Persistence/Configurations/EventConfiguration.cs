using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventApp
{
    public class EventConfiguration : IEntityTypeConfiguration<EventModel>
    {
        public void Configure(EntityTypeBuilder<EventModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).IsUnique();
            builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(1000);
            builder.Property(x => x.EventDateTime).IsRequired();
            builder.Property(x => x.Venue).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Category).IsRequired().HasMaxLength(100);
            builder.Property(x => x.MaxParticipants).IsRequired();

            builder.HasMany(e => e.Participants)
               .WithOne(ep => ep.Event)
               .HasForeignKey(ep => ep.EventId);
        }
    }
}
