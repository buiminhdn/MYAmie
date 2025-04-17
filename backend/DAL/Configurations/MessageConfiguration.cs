using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Configurations.Core;
using Models.Messages;
using Utility.Constants;

namespace DAL.Configurations;
public sealed class MessageConfiguration : BaseModelConfiguration<Message>
{
    public override void Configure(EntityTypeBuilder<Message> builder)
    {
        base.Configure(builder);

        builder.ToTable("Messages");

        // Property
        builder.Property(p => p.Content).HasMaxLength(ValidationLengths.Fields.Content);

        // Relationships
        builder.HasOne(m => m.Sender)
               .WithMany(a => a.SentMessages)
               .HasForeignKey(m => m.SenderId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Receiver)
               .WithMany(a => a.ReceivedMessages)
               .HasForeignKey(m => m.ReceiverId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
