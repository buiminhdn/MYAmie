namespace Common.ViewModels.PlaceVMs;
public class PlaceVM
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string Cover { get; set; }
    public string DateCreated { get; set; }
    public string City { get; set; }
    public int OwnerId { get; set; }
    public string OwnerAvatar { get; set; }
    public string OwnerName { get; set; }
}
