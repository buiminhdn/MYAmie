using Common.DTOs.Core;

namespace Common.DTOs.AccountDtos;
public class ChangePasswordParams : BaseParams
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}
