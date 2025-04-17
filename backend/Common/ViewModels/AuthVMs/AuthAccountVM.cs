namespace Common.ViewModels.AuthVMs;
public class AuthAccountVM
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Avatar { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public int Role { get; set; }

    // Access & RefreshToken
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
