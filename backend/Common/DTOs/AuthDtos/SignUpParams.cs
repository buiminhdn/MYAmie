namespace Common.DTOs.AuthDtos;
public class SignUpParams
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}
