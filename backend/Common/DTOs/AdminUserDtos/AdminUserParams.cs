using Common.DTOs.Core;
using Models.Accounts;
using Models.Core;

namespace Common.DTOs.AdminUserDtos;
public class AdminUserParams : PaginationParams
{
    public string SearchTerm { get; set; }
    public AccountStatus Status { get; set; }
    public Role Role { get; set; }
}
