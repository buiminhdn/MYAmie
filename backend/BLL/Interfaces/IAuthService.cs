using Common.DTOs.AuthDtos;
using Common.Responses;
using Common.ViewModels.AuthVMs;

namespace BLL.Interfaces;
public interface IAuthService
{
    Task<ApiResponse> SignUp(SignUpParams param);
    Task<ApiResponse> SignUpBusiness(SignUpBusinessParams param);
    Task<ApiResponse<AuthAccountVM>> SignIn(SignInParams param);
}
