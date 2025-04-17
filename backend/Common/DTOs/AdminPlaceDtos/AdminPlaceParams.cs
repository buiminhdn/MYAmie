using Common.DTOs.Core;
using Models.Places;

namespace Common.DTOs.AdminPlaceDtos;
public class AdminPlaceParams : PaginationParams
{
    public string SearchTerm { get; set; }
    public PlaceStatus Status { get; set; }
}
