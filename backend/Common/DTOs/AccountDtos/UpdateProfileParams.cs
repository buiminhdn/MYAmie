using Common.DTOs.Core;

namespace Common.DTOs.AccountDtos;
public class UpdateProfileParams : BaseParams
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int CityId { get; set; }
    public string Images { get; set; }
    public ICollection<string> Characteristics { get; set; }
    public ICollection<int> CategoryIds { get; set; }
}
