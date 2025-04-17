using Common.DTOs.Core;

namespace Common.DTOs.UserDtos;
public class FilterBusinessParams : PaginationParams
{
    public string SearchTerm { get; set; }
    public int? CityId { get; set; }
    public int? CategoryId { get; set; }
}
