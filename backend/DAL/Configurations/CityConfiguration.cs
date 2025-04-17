using DAL.Configurations.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Cities;
using System.Text.Json;
using Utility;
using Utility.Constants;

namespace DAL.Configurations;
public sealed class CityConfiguration : BaseModelConfiguration<City>
{
    public override void Configure(EntityTypeBuilder<City> builder)
    {
        base.Configure(builder);

        builder.ToTable("Cities");

        // Property
        builder.Property(c => c.Name)
            .HasMaxLength(ValidationLengths.Fields.Name);

        //Read JSON data from embedded resource
        string json = ResourceHelper.ReadEmbeddedResource("DAL.Resources.CityData.json");

        // Deserialize JSON into a List of City objects
        var cities = JsonSerializer.Deserialize<List<City>>(json);

        // Seed data only if cities exist
        if (cities != null && cities.Count > 0)
        {
            builder.HasData(cities);
        }

    }
}
