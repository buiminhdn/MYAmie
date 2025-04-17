using DAL.Configurations.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Categories;
using Utility.Constants;

namespace DAL.Configurations;
public sealed class CategoryConfiguration : BaseModelConfiguration<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);

        builder.ToTable("Categories");

        // Property
        builder.Property(c => c.Name)
            .HasMaxLength(ValidationLengths.Fields.Name);

        // Many-to-many relationships defined in Account and Place configurations
        builder.HasData(
            new Category { Id = 1, Name = "Thể thao", Icon = "fa-futbol" },
            new Category { Id = 2, Name = "Học tập", Icon = "fa-book" },
            new Category { Id = 3, Name = "Quay & Chụp", Icon = "fa-camera" },
            new Category { Id = 4, Name = "Làm đẹp", Icon = "fa-spa" },
            new Category { Id = 5, Name = "Du lịch", Icon = "fa-plane" },
            new Category { Id = 6, Name = "Thú cưng", Icon = "fa-paw" },
            new Category { Id = 7, Name = "Ăn uống", Icon = "fa-utensils" },
            new Category { Id = 8, Name = "Chơi game", Icon = "fa-game-console-handheld" },
            new Category { Id = 9, Name = "Ca hát", Icon = "fa-music-note" },
            new Category { Id = 10, Name = "Công nghệ", Icon = "fa-microchip" },
            new Category { Id = 11, Name = "Kinh doanh", Icon = "fa-briefcase-blank" }
        );

    }
}
