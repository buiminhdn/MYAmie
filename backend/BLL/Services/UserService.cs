using AutoMapper;
using BLL.Interfaces;
using Common.DTOs.UserDtos;
using Common.Pagination;
using Common.Responses;
using Common.ViewModels.BusinessVMs;
using Common.ViewModels.UserVMs;
using DAL.Repository.Core;
using Microsoft.Extensions.Logging;
using Models.Accounts;
using Models.Core;
using Models.Feedbacks;
using Models.Friendships;
using System.Linq.Expressions;
using Utility;
using Utility.Constants;

namespace BLL.Services;
public class UserService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserService> logger) : IUserService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<UserService> _logger = logger;

    public async Task<ApiResponse<PagedUsersVM>> GetUsers(FilterUserParams param)
    {
        try
        {
            var users = await _unitOfWork.Accounts.GetUsersWithinDistanceAsync(param);

            var userVMs = _mapper.Map<IEnumerable<UserVM>>(users, opt => opt.Items["FilterUserParams"] = param);

            if (param.CurrentUserId > 0)
            {
                var friendships = await _unitOfWork.Friendships.GetAllAsync(
                f => f.RequesterId == param.CurrentUserId || f.RequesteeId == param.CurrentUserId);

                foreach (var userVM in userVMs)
                {
                    // Find friendship between current user and this user
                    var friendship = friendships.FirstOrDefault(f =>
                        f.RequesterId == userVM.Id || f.RequesteeId == userVM.Id);

                    if (friendship != null)
                    {
                        userVM.FriendStatus = friendship.Status;
                        userVM.IsRequester = friendship.RequesterId == param.CurrentUserId;
                    }
                    else
                    {
                        userVM.FriendStatus = FriendshipStatus.NONE;
                        userVM.IsRequester = false;
                    }
                }
            }

            var pagedUsers = PagedList<UserVM>.ToPagedList(userVMs, param.PageNumber, param.PageSize);

            var pagedUsersVM = new PagedUsersVM
            {
                Users = pagedUsers,
                Pagination = pagedUsers.PaginationData
            };

            return ApiResponse<PagedUsersVM>.Success(pagedUsersVM);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error retrieving users with params: {@Param}", param);
            return ApiResponse<PagedUsersVM>.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse<PagedBusinessesVM>> GetBusinesses(FilterBusinessParams param)
    {
        try
        {
            var searchTerm = param.SearchTerm?.RemoveDiacritics() ?? string.Empty;

            Expression<Func<Account, bool>> filter = p =>
                p.Status == AccountStatus.ACTIVATED &&
                p.Role == Role.BUSINESS &&
                (string.IsNullOrEmpty(searchTerm) || p.NormalizedInfo.Contains(searchTerm)) &&
                (!param.CityId.HasValue || param.CityId == 0 || (param.CityId > 0 && p.CityId == param.CityId.Value)) &&
                (!param.CategoryId.HasValue || param.CategoryId == 0 || (param.CategoryId > 0 && p.Categories.Any(c => c.Id == param.CategoryId.Value)));

            // Fetch data using the repository's GetAll method
            var businesses = await _unitOfWork.Accounts.GetAllAsync(filter, "City,Business");

            var businessIds = businesses.Select(b => b.Id).ToList();

            var allFeedbacks = await _unitOfWork.Feedbacks.GetAllAsync(
            f => businessIds.Contains(f.TargetId) && f.TargetType == FeedbackTargetType.BUSINESS);

            var feedbackStats = allFeedbacks
            .GroupBy(f => f.TargetId)
            .ToDictionary(
                g => g.Key,
                g => new
                {
                    AverageRating = g.Average(f => f.Rating),
                    TotalFeedback = g.Count()
                });

            var businessVMs = businesses.Select(b =>
            {
                var vm = _mapper.Map<BusinessVM>(b);
                if (feedbackStats.TryGetValue(b.Id, out var stats))
                {
                    vm.AverageRating = (int)Math.Round(stats.AverageRating);
                    vm.TotalFeedback = stats.TotalFeedback;
                }
                return vm;
            }).ToList();

            var pagedBusinesses = PagedList<BusinessVM>.ToPagedList(businessVMs, param.PageNumber, param.PageSize);

            var pagedBusinessesVM = new PagedBusinessesVM
            {
                Businesses = pagedBusinesses,
                Pagination = pagedBusinesses.PaginationData
            };

            return ApiResponse<PagedBusinessesVM>.Success(pagedBusinessesVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving businesses with params: {@Param}", param);
            return ApiResponse<PagedBusinessesVM>.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse<UserDetailVM>> GetUserById(int id)
    {
        try
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(id, "City,Categories");

            if (account == null || account.Role != Role.USER)
                return ApiResponse<UserDetailVM>.Failure(ResponseMessages.UserNotFound);

            var userDetailVM = _mapper.Map<UserDetailVM>(account);

            return ApiResponse<UserDetailVM>.Success(userDetailVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with ID: {Id}", id);
            return ApiResponse<UserDetailVM>.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse<BusinessDetailVM>> GetBusinessById(int id)
    {
        try
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(id, "City,Categories,Business");

            if (account == null || account.Role != Role.BUSINESS)
                return ApiResponse<BusinessDetailVM>.Failure(ResponseMessages.BusinessNotFound);

            var businessVM = _mapper.Map<BusinessDetailVM>(account);

            return ApiResponse<BusinessDetailVM>.Success(businessVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving business with ID: {Id}", id);
            return ApiResponse<BusinessDetailVM>.Failure(ResponseMessages.UnexpectedError);
        }
    }
}
