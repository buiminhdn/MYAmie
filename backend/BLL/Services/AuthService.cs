using AutoMapper;
using BLL.Interfaces;
using Common.DTOs.AuthDtos;
using Common.Responses;
using Common.ViewModels.AuthVMs;
using DAL.Repository.Core;
using Microsoft.Extensions.Logging;
using Models.Accounts;
using Models.Businesses;
using Models.Core;
using Utility;
using Utility.Constants;

namespace BLL.Services;
public class AuthService(IUnitOfWork unitOfWork, ILogger<AuthService> logger, IMapper mapper) : IAuthService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<AuthService> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<ApiResponse<AuthAccountVM>> SignIn(SignInParams param)
    {
        try
        {
            var account = await _unitOfWork.Accounts.GetAsync(a => a.Email == param.Email);
            if (account == null)
                return ApiResponse<AuthAccountVM>.Failure(ResponseMessages.AccountNotFound);

            if (!account.IsEmailVerified)
                return ApiResponse<AuthAccountVM>.Failure(ResponseMessages.AccountNotVerified);

            if (account.Status == AccountStatus.SUSPENDED)
                return ApiResponse<AuthAccountVM>.Failure(ResponseMessages.AccountSuspended);

            if (!PasswordUtils.Verify(account.Password, param.Password))
                return ApiResponse<AuthAccountVM>.Failure(ResponseMessages.InvalidCredentials);

            var accountVM = _mapper.Map<AuthAccountVM>(account);

            return ApiResponse<AuthAccountVM>.Success(accountVM, ResponseMessages.LoginSuccess);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error signing in user {Email}", param.Email);
            return ApiResponse<AuthAccountVM>.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse> SignUp(SignUpParams param)
    {
        try
        {
            if (await _unitOfWork.Accounts.IsExistAsync(a => a.Email == param.Email))
                return ApiResponse.Failure(ResponseMessages.EmailAlreadyExists);

            var newAccount = new Account
            {
                Email = param.Email,
                Password = PasswordUtils.Hash(param.Password),
                LastName = param.LastName,
                FirstName = param.FirstName,
                NormalizedInfo = $"{param.LastName} {param.FirstName} {param.Email}".RemoveDiacritics(),
                Role = Role.USER,
                Latitude = param.Latitude,
                Longitude = param.Longitude,
                Categories = [],
            };

            await _unitOfWork.Accounts.AddAsync(newAccount);
            return await _unitOfWork.SaveAsync() > 0
                ? ApiResponse.Success(ResponseMessages.SignupSuccess)
                : ApiResponse.Failure(ResponseMessages.SignupFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error signing up user {Email}", param.Email);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse> SignUpBusiness(SignUpBusinessParams param)
    {
        try
        {
            if (await _unitOfWork.Accounts.IsExistAsync(a => a.Email == param.Email))
                return ApiResponse.Failure(ResponseMessages.EmailAlreadyExists);

            var city = await _unitOfWork.Cities.GetByIdAsync(param.CityId);

            if (city == null)
                return ApiResponse.Failure(ResponseMessages.CityNotFound);

            var categories = await _unitOfWork.Categories.GetAllAsync(c => param.CategoryIds.Contains(c.Id));

            if (categories.Count() != param.CategoryIds.Count)
                return ApiResponse.Failure(ResponseMessages.CategoryNotFound);

            var newAccount = new Account
            {
                Email = param.Email,
                Password = PasswordUtils.Hash(param.Password),
                FirstName = param.Name,
                ShortDescription = param.ShortDescription,
                NormalizedInfo = $"{param.Name} {param.Email} {param.ShortDescription}".RemoveDiacritics(),
                Role = Role.BUSINESS,
                City = city,
                Categories = categories.ToList(),
                Business = new Business(),
            };

            await _unitOfWork.Accounts.AddAsync(newAccount);
            return await _unitOfWork.SaveAsync() > 0
                ? ApiResponse.Success(ResponseMessages.SignupSuccess)
                : ApiResponse.Failure(ResponseMessages.SignupFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error signing up business {Email}", param.Email);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }


}
