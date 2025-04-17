using Common.DTOs.Core;

namespace Common.DTOs.AdminUserDtos;
public class AdminUserPasswordParams : BaseParams
{
    public int UserId { get; set; }
    public string Password { get; set; }
}