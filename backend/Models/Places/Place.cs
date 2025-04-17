using Models.Accounts;
using Models.Categories;
using Models.Cities;
using Models.Core;

namespace Models.Places;
public class Place : BaseModel
{
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public string NormalizedInfo { get; set; }
    public string Images { get; set; }
    public string Address { get; set; }
    public int ViewCount { get; set; }
    public PlaceStatus Status { get; set; } = PlaceStatus.ACTIVATED;
    public int OwnerId { get; set; }
    public int CityId { get; set; }


    // Navigation Properties
    public Account Owner { get; set; } // 1-to-1 with Account (owner)
    public City City { get; set; } // 1-to-1 with City
    public ICollection<Category> Categories { get; set; } // Many-to-many with Categories

}
