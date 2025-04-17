namespace Common.DTOs.EmailDtos;
public class RequestVerifyParams
{
    public string Email { get; set; }
    public VerificationTypeParam Type { get; set; }
}

public enum VerificationTypeParam
{
    AccountConfirmation = 1,
    PasswordReset = 2
}
