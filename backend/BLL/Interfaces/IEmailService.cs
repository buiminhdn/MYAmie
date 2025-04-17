using Common.DTOs.EmailDtos;
using Common.Responses;
using Common.ViewModels.EmailVMs;

namespace BLL.Interfaces;
public interface IEmailService
{
    Task<ApiResponse> SendVerifyEmail(string email, string verificationCode, VerificationTypeParam type);
    Task<ApiResponse> SetEmailVerified(string email);
    Task<ApiResponse<PagedEmailMarketingVM>> GetEmailMarketings(EmailMarketingParams param);
    Task<ApiResponse> AddMarketingEmail(string subject, string body);
    Task<ApiResponse> DeleteMarketingEmail(int id);
}
