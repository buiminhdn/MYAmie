using DAL.Configurations.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Feedbacks;
using Utility.Constants;

namespace DAL.Configurations;
public sealed class FeedbackConfiguration : BaseModelConfiguration<Feedback>
{
    public override void Configure(EntityTypeBuilder<Feedback> builder)
    {
        base.Configure(builder);

        builder.ToTable("Feedbacks");

        // Property
        builder.Property(f => f.Content)
            .HasMaxLength(ValidationLengths.Fields.Content);

        builder.Property(f => f.Response)
            .HasMaxLength(ValidationLengths.Fields.Content);

        builder.Property(f => f.TargetId).IsRequired();

        builder.Property(f => f.Rating).IsRequired();

        // Relationship
        builder.HasOne(f => f.Sender)
               .WithMany(a => a.SentFeedbacks)
               .HasForeignKey(f => f.SenderId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
