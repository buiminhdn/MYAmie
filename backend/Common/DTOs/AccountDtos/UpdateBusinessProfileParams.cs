using Common.DTOs.Core;

namespace Common.DTOs.AccountDtos;
public class UpdateBusinessProfileParams : BaseParams
{
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public int OpenHour { get; set; }
    public int CloseHour { get; set; }
    public string Phone { get; set; }
    public int CityId { get; set; }
    public ICollection<int> CategoryIds { get; set; }
    public string Images { get; set; }
}
