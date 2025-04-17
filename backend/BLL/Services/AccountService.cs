using AutoMapper;
using BLL.Interfaces;
using Common.DTOs.AccountDtos;
using Common.Responses;
using Common.ViewModels.AuthVMs;
using Common.ViewModels.ProfileVMs;
using DAL.Repository.Core;
using Microsoft.Extensions.Logging;
using Utility;
using Utility.Constants;

namespace BLL.Services;
public class AccountService(IUnitOfWork unitOfWork, ILogger<AccountService> logger, IMapper mapper) : IAccountService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<AccountService> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<ApiResponse> ChangePassword(ChangePasswordParams param)
    {
        try
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId);

            if (account == null)
                return ApiResponse.Failure(ResponseMessages.AccountNotFound);

            if (!account.IsEmailVerified)
                return ApiResponse.Failure(ResponseMessages.AccountNotVerified);

            if (!PasswordUtils.Verify(account.Password, param.OldPassword))
                return ApiResponse.Failure(ResponseMessages.OldPasswordIncorrect);

            account.Password = PasswordUtils.Hash(param.NewPassword);
            account.UpdatedBy = param.CurrentUserId;
            account.UpdatedDate = DateTimeUtils.TimeInEpoch();

            await _unitOfWork.Accounts.UpdateAsync(account);
            return await _unitOfWork.SaveAsync() > 0
                ? ApiResponse.Success(ResponseMessages.ChangePasswordSuccess)
                : ApiResponse.Failure(ResponseMessages.ChangePasswordFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password userId {id}", param.CurrentUserId);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse<BusinessProfileVM>> GetBusinessProfile(int id)
    {
        try
        {
            var business = await _unitOfWork.Accounts.GetByIdAsync(id, "Categories,City,Business");

            if (business == null)
                return ApiResponse<BusinessProfileVM>.Failure(ResponseMessages.AccountNotFound);

            var businessDto = _mapper.Map<BusinessProfileVM>(business);

            return ApiResponse<BusinessProfileVM>.Success(businessDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting profile userId {id}", id);
            return ApiResponse<BusinessProfileVM>.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse<UserProfileVM>> GetUserProfile(int id)
    {
        try
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(id, "Categories,City");

            if (account == null)
                return ApiResponse<UserProfileVM>.Failure(ResponseMessages.AccountNotFound);

            var userDto = _mapper.Map<UserProfileVM>(account);

            return ApiResponse<UserProfileVM>.Success(userDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting profile userId {id}", id);
            return ApiResponse<UserProfileVM>.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse> ResetPassword(string email, string password)
    {
        try
        {
            var account = await _unitOfWork.Accounts.GetAsync(a => a.Email == email);

            if (account == null)
                return ApiResponse<AuthAccountVM>.Failure(ResponseMessages.AccountNotFound);

            if (!account.IsEmailVerified)
                return ApiResponse<AuthAccountVM>.Failure(ResponseMessages.AccountNotVerified);

            account.Password = PasswordUtils.Hash(password);
            account.UpdatedBy = account.Id;
            account.UpdatedDate = DateTimeUtils.TimeInEpoch();

            await _unitOfWork.Accounts.UpdateAsync(account);
            return await _unitOfWork.SaveAsync() > 0
                ? ApiResponse.Success(ResponseMessages.ResetPasswordSuccess)
                : ApiResponse.Failure(ResponseMessages.ResetPasswordFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reseting password {Email}", email);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse> UpdateProfile(UpdateProfileParams param)
    {
        try
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, "Categories,City");

            if (account == null)
                return ApiResponse.Failure(ResponseMessages.AccountNotFound);


            if (!account.IsEmailVerified)
                return ApiResponse.Failure(ResponseMessages.AccountNotVerified);

            if (param.CityId > 0 && account.CityId != param.CityId)
            {
                var city = await _unitOfWork.Cities.GetByIdAsync(param.CityId);

                if (city == null)
                    return ApiResponse.Failure(ResponseMessages.CityNotFound);

                account.City = city;
            }

            if (param.CategoryIds?.Count > 0)
            {
                var categories = await _unitOfWork.Categories.GetAllAsync(c => param.CategoryIds.Contains(c.Id));
                account.Categories = categories.ToList();
            }

            account.Images = param.Images;
            account.FirstName = param.FirstName;
            account.LastName = param.LastName;
            account.ShortDescription = param.ShortDescription;
            account.Description = param.Description;
            account.NormalizedInfo = $"{param.FirstName} {param.LastName} {account.Email} {param.ShortDescription}".RemoveDiacritics();
            account.DateOfBirth = DateTimeUtils.TimeInEpoch(param.DateOfBirth);
            account.UpdatedBy = param.CurrentUserId;
            account.UpdatedDate = DateTimeUtils.TimeInEpoch();
            account.Characteristics = param.Characteristics?.Count > 0
                ? string.Join(";", param.Characteristics)
                : null;

            await _unitOfWork.Accounts.UpdateAsync(account);

            return await _unitOfWork.SaveAsync() > 0
                ? ApiResponse.Success(ResponseMessages.UpdateProfileSuccess)
                : ApiResponse.Failure(ResponseMessages.UpdateProfileFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating profile for UserId {UserId}", param.CurrentUserId);
            return ApiResponse<UserProfileVM>.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse> UpdateBusinessProfile(UpdateBusinessProfileParams param)
    {
        try
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId, "Categories,City,Business");

            if (account == null)
                return ApiResponse.Failure(ResponseMessages.AccountNotFound);

            if (!account.IsEmailVerified)
                return ApiResponse.Failure(ResponseMessages.AccountNotVerified);

            if (param.CityId > 0 && account.CityId != param.CityId)
            {
                var city = await _unitOfWork.Cities.GetByIdAsync(param.CityId);

                if (city == null)
                    return ApiResponse.Failure(ResponseMessages.CityNotFound);

                account.City = city;
            }

            if (param.CategoryIds?.Count > 0)
            {
                var categories = await _unitOfWork.Categories.GetAllAsync(c => param.CategoryIds.Contains(c.Id));
                account.Categories = categories.ToList();
            }

            account.Images = param.Images;
            account.FirstName = param.Name;
            account.ShortDescription = param.ShortDescription;
            account.Description = param.Description;
            account.NormalizedInfo = $"{param.Name} {account.Email} {param.ShortDescription}".RemoveDiacritics();
            account.UpdatedBy = param.CurrentUserId;
            account.UpdatedDate = DateTimeUtils.TimeInEpoch();
            account.Business.Address = param.Address;
            account.Business.OperatingHours = $"{param.OpenHour}-{param.CloseHour}";
            account.Business.Phone = param.Phone;
            account.Business.UpdatedBy = param.CurrentUserId;
            account.Business.UpdatedDate = DateTimeUtils.TimeInEpoch();

            await _unitOfWork.Accounts.UpdateAsync(account);

            return await _unitOfWork.SaveAsync() > 0
                ? ApiResponse.Success(ResponseMessages.UpdateProfileSuccess)
                : ApiResponse.Failure(ResponseMessages.UpdateProfileFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating profile for BusinessId {UserId}", param.CurrentUserId);
            return ApiResponse<BusinessProfileVM>.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse<UpdateAvatarOrCoverVM>> UpdateAvatarOrCover(int accountId, string imgPath, bool isAvatar)
    {
        try
        {
            if (string.IsNullOrEmpty(imgPath))
                return ApiResponse<UpdateAvatarOrCoverVM>.Failure(ResponseMessages.NoImagesProvided);

            var account = await _unitOfWork.Accounts.GetByIdAsync(accountId);

            if (account == null)
                return ApiResponse<UpdateAvatarOrCoverVM>.Failure(ResponseMessages.AccountNotFound);

            if (!account.IsEmailVerified)
                return ApiResponse<UpdateAvatarOrCoverVM>.Failure(ResponseMessages.AccountNotVerified);

            var oldImage = isAvatar ? account.Avatar : account.Cover;

            if (isAvatar)
                account.Avatar = imgPath;
            else
                account.Cover = imgPath;

            account.UpdatedBy = accountId;
            account.UpdatedDate = DateTimeUtils.TimeInEpoch();

            await _unitOfWork.Accounts.UpdateAsync(account);
            bool isSaved = await _unitOfWork.SaveAsync() > 0;

            if (!isSaved)
                return ApiResponse<UpdateAvatarOrCoverVM>.Failure(ResponseMessages.UpdateImageFailed);

            return ApiResponse<UpdateAvatarOrCoverVM>.Success(
            new UpdateAvatarOrCoverVM
            {
                OldPath = oldImage,
                NewPath = imgPath
            },
            ResponseMessages.UpdateImageSuccess);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating image for UserId {UserId}", accountId);
            return ApiResponse<UpdateAvatarOrCoverVM>.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse> UpdateLocation(UpdateLocationParams param)
    {
        try
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(param.CurrentUserId);

            if (account == null)
                return ApiResponse.Failure(ResponseMessages.AccountNotFound);

            if (!account.IsEmailVerified)
                return ApiResponse.Failure(ResponseMessages.AccountNotVerified);

            account.Latitude = param.Latitude;
            account.Longitude = param.Longitude;

            await _unitOfWork.Accounts.UpdateAsync(account);
            return await _unitOfWork.SaveAsync() > 0
                ? ApiResponse.Success(ResponseMessages.UpdateLocationSuccess)
                : ApiResponse.Failure(ResponseMessages.UpdateLocationFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating location for UserId {UserId}", param.CurrentUserId);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse<AvatarWNameVM>> GetAvatarWName(int userId)
    {
        try
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(userId);

            if (account == null)
                return ApiResponse<AvatarWNameVM>.Failure(ResponseMessages.AccountNotFound);

            AvatarWNameVM accountVM = new()
            {
                Id = account.Id,
                Name = account.FirstName,
                Avatar = account.Avatar
            };

            return ApiResponse<AvatarWNameVM>.Success(accountVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting avatar with name for UserId {UserId}", userId);
            return ApiResponse<AvatarWNameVM>.Failure(ResponseMessages.UnexpectedError);
        }
    }
}
