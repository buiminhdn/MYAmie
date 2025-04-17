using BLL.Interfaces;
using Common.Responses;
using DAL.Repository.Core;
using Microsoft.Extensions.Logging;
using Utility;
using Utility.Constants;

namespace BLL.Services;
public class TokenService(IUnitOfWork unitOfWork, ILogger<AuthService> logger) : ITokenService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<AuthService> _logger = logger;

    public async Task<ApiResponse<string>> IsRefreshTokenValid(int accountId, string refreshToken)
    {
        try
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(accountId);

            if (account == null)
                return ApiResponse<string>.Failure(ResponseMessages.AccountNotFound);

            if (account.RefreshToken != refreshToken || account.RefreshTokenExpiryTime < DateTimeUtils.TimeInEpoch(DateTime.Now))
                return ApiResponse<string>.Failure(ResponseMessages.InvalidRefreshToken);

            return ApiResponse<string>.Success(account.Email, ResponseMessages.ValidRefreshToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while validating refresh token for user {AccountId}", accountId);
            return ApiResponse<string>.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse> RevokeRefreshToken(int accountId)
    {
        try
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(accountId);

            if (account == null)
                return ApiResponse.Failure(ResponseMessages.AccountNotFound);

            account.RefreshToken = null;

            await _unitOfWork.Accounts.UpdateAsync(account);
            return await _unitOfWork.SaveAsync() > 0
                ? ApiResponse.Success(ResponseMessages.LogoutSuccess)
                : ApiResponse.Failure(ResponseMessages.LogoutFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while revoking refresh token for user {AccountId}", accountId);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse> StoreRefreshToken(int accountId, string refreshToken)
    {
        try
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(accountId);

            if (account == null)
                return ApiResponse.Failure(ResponseMessages.AccountNotFound);

            account.RefreshToken = refreshToken;
            account.RefreshTokenExpiryTime = DateTimeUtils.TimeInEpoch(DateTime.Now.AddDays(30));

            await _unitOfWork.Accounts.UpdateAsync(account);
            return await _unitOfWork.SaveAsync() > 0
               ? ApiResponse.Success()
               : ApiResponse.Failure();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while storing refresh token for user {AccountId}", accountId);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }
}
