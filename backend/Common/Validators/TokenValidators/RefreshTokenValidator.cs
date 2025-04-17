using Common.DTOs.TokenDtos;
using FluentValidation;
using Utility.Constants;

namespace Common.Validators.TokenValidators;
public class RefreshTokenValidator : AbstractValidator<RefreshTokenParams>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.AccessToken)
               .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Token"));

        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage(string.Format(ValidationMessages.RequiredField, "Refresh Token"));
    }
}
