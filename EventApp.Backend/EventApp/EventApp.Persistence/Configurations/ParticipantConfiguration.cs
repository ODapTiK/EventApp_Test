using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventApp
{
    public class ParticipantConfiguration : IEntityTypeConfiguration<ParticipantModel>
    {
        public void Configure(EntityTypeBuilder<ParticipantModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).IsUnique();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Surname).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(150);
            builder.Property(x => x.BirthDate).IsRequired();
            builder.Property(x => x.Password).IsRequired();

            builder.HasMany(p => p.Events)
               .WithOne(ep => ep.Participant)
               .HasForeignKey(ep => ep.ParticipantId);
        }
    }
}
