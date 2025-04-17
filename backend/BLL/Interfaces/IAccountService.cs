using Common.DTOs.AccountDtos;
using Common.Responses;
using Common.ViewModels.ProfileVMs;

namespace BLL.Interfaces;
public interface IAccountService
{
    Task<ApiResponse> ResetPassword(string email, string password);
    Task<ApiResponse<UserProfileVM>> GetUserProfile(int id);
    Task<ApiResponse<BusinessProfileVM>> GetBusinessProfile(int id);
    Task<ApiResponse> ChangePassword(ChangePasswordParams param);
    Task<ApiResponse> UpdateProfile(UpdateProfileParams param);
    Task<ApiResponse> UpdateBusinessProfile(UpdateBusinessProfileParams param);
    Task<ApiResponse<UpdateAvatarOrCoverVM>> UpdateAvatarOrCover(int accountId, string imgPath, bool isAvatar);
    Task<ApiResponse> UpdateLocation(UpdateLocationParams param);
    Task<ApiResponse<AvatarWNameVM>> GetAvatarWName(int userId);
}
