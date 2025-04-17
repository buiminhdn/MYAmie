using Common.Responses;

namespace BLL.Interfaces;
public interface ITokenService
{
    Task<ApiResponse> StoreRefreshToken(int accountId, string refreshToken);
    Task<ApiResponse> RevokeRefreshToken(int accountId);
    Task<ApiResponse<string>> IsRefreshTokenValid(int accountId, string refreshToken);
}
