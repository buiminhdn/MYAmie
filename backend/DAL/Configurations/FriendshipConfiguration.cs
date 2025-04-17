using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Configurations.Core;
using Models.Friendships;

namespace DAL.Configurations;
public sealed class FriendshipConfiguration : BaseModelConfiguration<Friendship>
{
    public override void Configure(EntityTypeBuilder<Friendship> builder)
    {
        base.Configure(builder);

        builder.ToTable("Friendships");


        // Relationships
        builder.HasOne(f => f.Requester)
               .WithMany(a => a.RequestedFriendships)
               .HasForeignKey(f => f.RequesterId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Requestee)
               .WithMany(a => a.ReceivedFriendships)
               .HasForeignKey(f => f.RequesteeId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
