using Common.DTOs.AdminUserDtos;
using Common.Responses;
using Common.ViewModels.AdminUserVMs;

namespace BLL.Interfaces;
public interface IAdminUserService
{
    Task<ApiResponse<PagedAdminUsersVM>> GetUsersByAdmin(AdminUserParams param);
    Task<ApiResponse> UpdateUserStatus(AdminUserStatusParams param);
    Task<ApiResponse> UpdateUserPassword(AdminUserPasswordParams param);

}
