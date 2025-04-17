using Common.DTOs.Core;
using Models.Places;

namespace Common.DTOs.AdminPlaceDtos;
public class AdminPlaceStatusParams : BaseParams
{
    public int PlaceId { get; set; }
    public PlaceStatus Status { get; set; }
}
