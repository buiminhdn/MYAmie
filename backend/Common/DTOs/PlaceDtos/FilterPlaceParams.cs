using Common.DTOs.Core;

namespace Common.DTOs.PlaceDtos;
public class FilterPlaceParams : PaginationParams
{
    public string SearchTerm { get; set; }
    public int? CityId { get; set; }
    public int? CategoryId { get; set; }
}