using Common.DTOs.Core;

namespace Common.DTOs.AccountDtos;
public class UpdateLocationParams : BaseParams
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}
