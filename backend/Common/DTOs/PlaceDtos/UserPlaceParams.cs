using Common.DTOs.Core;

namespace Common.DTOs.PlaceDtos;
public class UserPlaceParams : PaginationParams
{
    public int UserId { get; set; }
}
