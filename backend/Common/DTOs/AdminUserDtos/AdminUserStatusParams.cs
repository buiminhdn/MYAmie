using Common.DTOs.Core;
using Models.Accounts;

namespace Common.DTOs.AdminUserDtos;
public class AdminUserStatusParams : BaseParams
{
    public int UserId { get; set; }
    public AccountStatus Status { get; set; }
}
