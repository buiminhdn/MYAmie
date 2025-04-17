using DAL.Configurations.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Accounts;
using Models.Businesses;
using Models.Core;
using Utility;
using Utility.Constants;

namespace DAL.Configurations;
public sealed class AccountConfiguration : BaseModelConfiguration<Account>
{
    public override void Configure(EntityTypeBuilder<Account> builder)
    {
        base.Configure(builder);

        builder.ToTable("Accounts");

        // Property
        builder.Property(p => p.Email).HasMaxLength(ValidationLengths.MaxLengthDefault);
        builder.Property(p => p.LastName).HasMaxLength(ValidationLengths.Fields.Name);
        builder.Property(p => p.FirstName).HasMaxLength(ValidationLengths.Fields.Name);

        builder.Property(p => p.Password).HasMaxLength(ValidationLengths.MaxLengthDefault);

        builder.Property(a => a.Latitude).HasColumnType("decimal(8, 6)");
        builder.Property(a => a.Longitude).HasColumnType("decimal(9, 6)");

        // Relationship
        builder.HasOne(a => a.City)
               .WithMany(c => c.Accounts)
               .HasForeignKey(a => a.CityId)
               .OnDelete(DeleteBehavior.Restrict).IsRequired(false);

        builder.HasOne(a => a.Business)
               .WithOne(b => b.Owner)
               .HasForeignKey<Business>(b => b.OwnerId)
               .OnDelete(DeleteBehavior.Cascade).IsRequired(false);

        builder.HasMany(a => a.Places)
               .WithOne(p => p.Owner)
               .HasForeignKey(p => p.OwnerId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.SentFeedbacks)
               .WithOne(f => f.Sender)
               .HasForeignKey(f => f.SenderId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.RequestedFriendships)
               .WithOne(f => f.Requester)
               .HasForeignKey(f => f.RequesterId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.ReceivedFriendships)
               .WithOne(f => f.Requestee)
               .HasForeignKey(f => f.RequesteeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.SentMessages)
               .WithOne(m => m.Sender)
               .HasForeignKey(m => m.SenderId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.ReceivedMessages)
               .WithOne(m => m.Receiver)
               .HasForeignKey(m => m.ReceiverId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.Categories)
               .WithMany(c => c.Accounts);

        builder.HasData(new Account
        {
            Id = 1,
            Email = "minhbee203@gmail.com",
            Password = PasswordUtils.Hash("Nhatkycuaminh2024."),
            FirstName = "Minh",
            LastName = "Bùi",
            Role = Role.ADMIN,
            IsEmailVerified = true,
        });

        // Sample Data
        builder.HasData(
             new Account
             {
                 Id = 2,
                 Email = "user1@gmail.com",
                 Password = PasswordUtils.Hash("123456"),
                 FirstName = "1",
                 LastName = "User",
                 Role = Role.USER,
                 IsEmailVerified = true,
                 Avatar = "avt1.png",
                 Latitude = 16.09932790307512m,
                 Longitude = 108.24420359528237m
             },
             new Account
             {
                 Id = 3,
                 Email = "user2@gmail.com",
                 Password = PasswordUtils.Hash("123456"),
                 FirstName = "2",
                 LastName = "User",
                 Role = Role.USER,
                 IsEmailVerified = true,
                 Avatar = "avt2.png",
                 Latitude = 16.086021861878294m,
                 Longitude = 108.21735108528173m
             }
         );

    }
}
