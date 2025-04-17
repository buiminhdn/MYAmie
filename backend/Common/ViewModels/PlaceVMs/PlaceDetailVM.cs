using Common.ViewModels.CategoryVMs;
using Common.ViewModels.CityVMs;

namespace Common.ViewModels.PlaceVMs;
public class PlaceDetailVM
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public IEnumerable<string> Images { get; set; }
    public string Address { get; set; }
    public int ViewCount { get; set; }

    // Relationship
    public CityVM City { get; set; }
    public int OwnerId { get; set; }
    public string OwnerAvatar { get; set; }
    public string OwnerName { get; set; }
    public string DateCreated { get; set; }
    public IEnumerable<CategoryVM> Categories { get; set; }
}
