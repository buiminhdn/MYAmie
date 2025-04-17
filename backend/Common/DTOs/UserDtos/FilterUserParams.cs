using Common.DTOs.Core;

namespace Common.DTOs.UserDtos;
public class FilterUserParams : PaginationParams
{
    public int? CategoryId { get; set; }
    public int? DistanceInKm { get; set; } = 5;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}
