using DAL.Configurations.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Places;
using Utility.Constants;

namespace DAL.Configurations;
public sealed class PlaceConfiguration : BaseModelConfiguration<Place>
{
    public override void Configure(EntityTypeBuilder<Place> builder)
    {
        base.Configure(builder);

        builder.ToTable("Places");

        // Property
        builder.Property(p => p.Name)
            .HasMaxLength(ValidationLengths.Fields.Name)
            .IsRequired();

        builder.Property(p => p.Address)
            .HasMaxLength(ValidationLengths.Fields.Address);

        // Relationships
        builder.HasOne(p => p.Owner)
               .WithMany(a => a.Places)
               .HasForeignKey(p => p.OwnerId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.City)
               .WithMany(c => c.Places)
               .HasForeignKey(p => p.CityId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Categories)
               .WithMany(c => c.Places);
    }
}
