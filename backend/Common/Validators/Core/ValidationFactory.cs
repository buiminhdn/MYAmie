using Common.DTOs.AccountDtos;
using Common.DTOs.AdminPlaceDtos;
using Common.DTOs.AdminUserDtos;
using Common.DTOs.AuthDtos;
using Common.DTOs.ChatDtos;
using Common.DTOs.Core;
using Common.DTOs.EmailDtos;
using Common.DTOs.FeedbackDtos;
using Common.DTOs.FriendshipDtos;
using Common.DTOs.PlaceDtos;
using Common.DTOs.TokenDtos;
using Common.DTOs.UserDtos;
using Common.Validators.AccountValidators;
using Common.Validators.AdminPlaceValidators;
using Common.Validators.AdminUserValidators;
using Common.Validators.AuthValidators;
using Common.Validators.ChatValidators;
using Common.Validators.EmailValidators;
using Common.Validators.FeedbackValidators;
using Common.Validators.FriendshipValidators;
using Common.Validators.PlaceValidators;
using Common.Validators.TokenValidators;
using Common.Validators.UserValidators;
using FluentValidation;
using FluentValidation.Results;
using MYAmie.Common.DTOs.FeedbackDtos;

namespace Common.Validators.Core;
public class ValidationFactory : IValidationFactory
{
    private static readonly Dictionary<Type, Type> ValidatorMap = new()
    {
        { typeof(BaseParams), typeof(BaseParamsValidator) },
        // Auth
        { typeof(SignUpParams), typeof(SignUpValidator) },
        { typeof(SignInParams), typeof(SignInValidator) },
        { typeof(RefreshTokenParams), typeof(RefreshTokenValidator) },
        { typeof(SignUpBusinessParams), typeof(SignUpBusinessValidator) },
        // Email
        { typeof(RequestVerifyParams), typeof(RequestVerifyValidator) },
        { typeof(VerifyEmailParams), typeof(VerifyEmailValidator) },
        // Account
        { typeof(ResetPasswordParams), typeof(ResetPasswordValidator) },
        { typeof(ChangePasswordParams), typeof(ChangePasswordValidator) },
        { typeof(UpdateProfileParams), typeof(UpdateProfileValidator) },
        { typeof(UpdateBusinessProfileParams), typeof(UpdateBusinessProfileValidator) },
        { typeof(UpdateAvatarOrCoverParams), typeof(UpdateAvatarOrCoverValidator) },
        { typeof(UpdateLocationParams), typeof(UpdateLocationValidator) },
        // Place
        { typeof(UpsertPlaceParams), typeof(UpsertPlaceValidator) },
        { typeof(DeletePlaceParams), typeof(DeletePlaceValidator) },
        { typeof(UserPlaceParams), typeof(UserPlaceValidator) },
        { typeof(FilterUserParams), typeof(FilterUserValidator) },
        { typeof(PagedMessageParams), typeof(PagedMessageValidator) },
        { typeof(FriendRequestParams), typeof(FriendRequestValidator) },
        { typeof(DeleteMarketingEmailParams), typeof(DeleteMarketingEmailValidator) },
        // Feedback
        { typeof(AddFeedbackParams), typeof(AddFeedbackValidator) },
        { typeof(UpdateFeedbackParams), typeof(UpdateFeedbackValidator) },
        { typeof(DeleteFeedbackParams), typeof(DeleteFeedbackValidator) },
        { typeof(ResponseFeedbackParams), typeof(ResponseFeedbackValidator) },
        { typeof(FilterFeedbackParams), typeof(FilterFeedbackValidator) },
        // Admin User
        { typeof(AdminUserPasswordParams), typeof(AdminUserPasswordValidator) },
        { typeof(AdminUserStatusParams), typeof(AdminUserStatusValidator) },
        { typeof(AddMarketingEmailParams), typeof(AddMarketingEmailValidator) },
        // Admin Place
        { typeof(AdminPlaceStatusParams), typeof(AdminPlaceStatusValidator) }
    };
    private static IValidator<T> GetValidator<T>()
    {
        if (ValidatorMap.TryGetValue(typeof(T), out Type validatorType))
        {
            return (IValidator<T>)Activator.CreateInstance(validatorType);
        }

        throw new NotImplementedException($"Validator for type {typeof(T).Name} is not implemented.");
    }

    public ValidationResult Validate<T>(T data)
    {
        IValidator<T> validator = GetValidator<T>();
        return validator.Validate(data);
    }

    public async Task<ValidationResult> ValidateAsync<T>(T data)
    {
        IValidator<T> validator = GetValidator<T>();
        return await validator.ValidateAsync(data);
    }
}
