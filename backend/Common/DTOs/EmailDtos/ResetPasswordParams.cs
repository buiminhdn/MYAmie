namespace Common.DTOs.EmailDtos;
public class ResetPasswordParams
{
    public string Email { get; set; }
    public string Code { get; set; }
    public string NewPassword { get; set; }
}
