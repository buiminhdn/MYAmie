namespace Models.Emails;
public enum EmailStatus
{
    PENDING = 1,
    SENT = 2,
    FAILED = 3
}
public enum EmailType
{
    GENERAL = 1,
    PASSWORDRESET = 2,
    VERIFICATION = 3,
    MARKETING = 4,
}
