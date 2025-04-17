using AutoMapper;
using BLL.Interfaces;
using Common.DTOs.AdminUserDtos;
using Common.Pagination;
using Common.Responses;
using Common.ViewModels.AdminUserVMs;
using Common.ViewModels.UserVMs;
using DAL.Repository.Core;
using Microsoft.Extensions.Logging;
using Models.Accounts;
using Models.Core;
using System.Linq.Expressions;
using Utility;
using Utility.Constants;

namespace BLL.Services;
public class AdminUserService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AdminUserService> logger) : IAdminUserService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<AdminUserService> _logger = logger;

    public async Task<ApiResponse<PagedAdminUsersVM>> GetUsersByAdmin(AdminUserParams param)
    {
        try
        {
            // Validate status and role parameters
            if (param.Status != 0 && !Enum.IsDefined(typeof(AccountStatus), param.Status))
                return ApiResponse<PagedAdminUsersVM>.Failure(ResponseMessages.InvalidStatus);

            if ((param.Role != 0 && !Enum.IsDefined(typeof(Role), param.Role)) || param.Role == Role.ADMIN)
                return ApiResponse<PagedAdminUsersVM>.Failure(ResponseMessages.InvalidRole);

            // Build the filter expression
            Expression<Func<Account, bool>> filter = account =>
                account.Role != Role.ADMIN &&
                (param.Status == 0 || account.Status == param.Status) &&
                (param.Role == 0 || account.Role == param.Role) &&
                //(!param.CityId.HasValue || account.CityId == param.CityId.Value) &&
                (string.IsNullOrEmpty(param.SearchTerm) ||
                    account.NormalizedInfo.Contains(param.SearchTerm.RemoveDiacritics()));

            var users = await _unitOfWork.Accounts.GetAllAsync(filter, "City");

            var userVMs = _mapper.Map<IEnumerable<AdminUserVM>>(users);

            var pagedUsers = PagedList<AdminUserVM>.ToPagedList(userVMs, param.PageNumber, param.PageSize);

            var pagedUsersVM = new PagedAdminUsersVM
            {
                Users = pagedUsers,
                Pagination = pagedUsers.PaginationData
            };

            return ApiResponse<PagedAdminUsersVM>.Success(pagedUsersVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users by admin with params: {@Param}", param);
            return ApiResponse<PagedAdminUsersVM>.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse> UpdateUserPassword(AdminUserPasswordParams param)
    {
        try
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(param.UserId);

            if (account == null)
                return ApiResponse<UserDetailVM>.Failure(ResponseMessages.UserNotFound);

            account.Password = PasswordUtils.Hash(param.Password);
            account.UpdatedBy = param.CurrentUserId;
            account.UpdatedDate = DateTimeUtils.TimeInEpoch();

            await _unitOfWork.Accounts.UpdateAsync(account);

            return await _unitOfWork.SaveAsync() > 0 ?
                ApiResponse.Success(ResponseMessages.ChangePasswordSuccess)
                : ApiResponse.Success(ResponseMessages.ChangePasswordFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user password for UserId {userId}", param.UserId);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse> UpdateUserStatus(AdminUserStatusParams param)
    {
        try
        {
            if (param.Status != 0 && !Enum.IsDefined(typeof(AccountStatus), param.Status))
                return ApiResponse<PagedAdminUsersVM>.Failure(ResponseMessages.InvalidStatus);

            var account = await _unitOfWork.Accounts.GetByIdAsync(param.UserId);

            if (account == null)
                return ApiResponse<UserDetailVM>.Failure(ResponseMessages.UserNotFound);

            account.Status = param.Status;
            account.UpdatedBy = param.CurrentUserId;
            account.UpdatedDate = DateTimeUtils.TimeInEpoch();

            await _unitOfWork.Accounts.UpdateAsync(account);

            return await _unitOfWork.SaveAsync() > 0 ?
                ApiResponse.Success(ResponseMessages.UpdateStatusSuccess)
                : ApiResponse.Failure(ResponseMessages.UpdateStatusFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user status for UserId {userId}", param.UserId);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }
}
