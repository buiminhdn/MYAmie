using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Configurations.Core;
using Models.Emails;

namespace DAL.Configurations;
public sealed class EmailConfiguration : BaseModelConfiguration<Email>
{
    public override void Configure(EntityTypeBuilder<Email> builder)
    {
        base.Configure(builder);

        builder.ToTable("Emails");
    }
}
