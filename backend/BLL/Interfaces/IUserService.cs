using Common.DTOs.UserDtos;
using Common.Responses;
using Common.ViewModels.BusinessVMs;
using Common.ViewModels.UserVMs;

namespace BLL.Interfaces;
public interface IUserService
{
    Task<ApiResponse<PagedUsersVM>> GetUsers(FilterUserParams param);
    Task<ApiResponse<UserDetailVM>> GetUserById(int id);
    Task<ApiResponse<PagedBusinessesVM>> GetBusinesses(FilterBusinessParams param);
    Task<ApiResponse<BusinessDetailVM>> GetBusinessById(int id);
}
