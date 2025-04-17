namespace Common.DTOs.AuthDtos;
public class SignUpBusinessParams
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ShortDescription { get; set; }
    public string Name { get; set; }
    public int CityId { get; set; }
    public ICollection<int> CategoryIds { get; set; }
}
