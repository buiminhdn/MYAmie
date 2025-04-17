namespace Common.ViewModels.ProfileVMs;
public class BusinessProfileVM
{
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public string Avatar { get; set; }
    public string Cover { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public int OpenHour { get; set; }
    public int CloseHour { get; set; }
    public ICollection<string> Images { get; set; }
    public int CityId { get; set; }
    public ICollection<int> CategoryIds { get; set; }
}
