using Common.DTOs.Core;

namespace Common.DTOs.PlaceDtos;
public class UpsertPlaceParams : BaseParams
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public int CityId { get; set; }
    public ICollection<int> CategoryIds { get; set; } = [];
    public string Address { get; set; }
    public string Description { get; set; }
    public string Images { get; set; }

}
