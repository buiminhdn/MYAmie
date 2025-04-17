using DAL.Configurations.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Businesses;
using Utility.Constants;

namespace DAL.Configurations;
public sealed class BusinessConfiguration : BaseModelConfiguration<Business>
{
    public override void Configure(EntityTypeBuilder<Business> builder)
    {
        base.Configure(builder);

        builder.ToTable("Businesses");

        // Property
        builder.Property(b => b.Address)
            .HasMaxLength(ValidationLengths.Fields.Address);

        builder.Property(b => b.Phone)
            .HasMaxLength(ValidationLengths.Fields.Phone);

        builder.Property(b => b.OperatingHours)
            .HasMaxLength(ValidationLengths.Fields.OperatingHours);
    }
}
