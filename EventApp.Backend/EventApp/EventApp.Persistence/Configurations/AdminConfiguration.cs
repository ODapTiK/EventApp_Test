using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventApp
{
    public class AdminConfiguration : IEntityTypeConfiguration<AdminModel>
    {
        public void Configure(EntityTypeBuilder<AdminModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).IsUnique();
            builder.Property(x => x.Email).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Password).IsRequired();
        }
    }
}
