namespace Utility.Constants;
public static class ResponseMessages
{
    public const string Success = "Success";
    public const string Failed = "Failed";
    public const string InvalidId = "ID không hợp lệ";
    public const string InvalidStatus = "Trạng thái không hợp lệ";
    public const string InvalidRole = "Vai trò không hợp lệ";

    // Common messages
    public const string LoginRequired = "Vui lòng đăng nhập lại";
    public const string NotAllowed = "Không được phép";
    public const string NotFound = "Không tìm thấy";
    public const string NoData = "Không có dữ liệu";
    public const string UnexpectedError = "Có lỗi xảy ra. Vui lòng thử lại sau.";

    // Authentication
    public const string SignupSuccess = "Đăng ký thành công";
    public const string SignupFailed = "Đăng ký thất bại";
    public const string LoginSuccess = "Đăng nhập thành công";
    public const string LoginFailed = "Đăng nhập thất bại";
    public const string LogoutSuccess = "Đăng xuất thành công";
    public const string LogoutFailed = "Đăng xuất thất bại";
    public const string InvalidCredentials = "Thông tin đăng nhập không chính xác";
    public const string AccountNotFound = "Tài khoản không tồn tại";
    public const string AccountNotVerified = "Tài khoản chưa được xác thực";
    public const string AccountSuspended = "Tài khoản đã bị khóa";
    public const string EmailAlreadyExists = "Tài khoản đã tồn tại";

    // Password
    public const string ChangePasswordSuccess = "Đổi mật khẩu thành công";
    public const string ChangePasswordFailed = "Đổi mật khẩu thất bại";
    public const string ResetPasswordSuccess = "Đặt lại mật khẩu thành công";
    public const string ResetPasswordFailed = "Đặt lại mật khẩu thất bại";
    public const string OldPasswordIncorrect = "Mật khẩu cũ không chính xác";

    // Email
    public const string EmailSent = "Email đã được gửi";
    public const string EmailNotSent = "Email không được gửi";
    public const string EmailVerified = "Email đã được xác thực";
    public const string EmailNotVerified = "Email chưa được xác thực";
    public const string ExpiredVerificationCode = "Mã xác minh đã hết hạn hoặc không hợp lệ";
    public const string EmailNotFound = "Email không tồn tại";

    public const string MarketingEmailCreated = "Email marketing đã được thêm thành công.";
    public const string MarketingEmailNotCreated = "Không thể tạo email marketing.";
    public const string MarketingEmailSentSuccess = "Email marketing đã được gửi thành công.";
    public const string MarketingEmailSentFailure = "Không thể gửi email marketing.";
    public const string EmailDeleted = "Xóa email thành công";
    public const string EmailNotDeleted = "Xóa email thất bại";

    // Place
    public const string AddPlaceSuccess = "Thêm địa điểm thành công";
    public const string AddPlaceFailed = "Thêm địa điểm thất bại";
    public const string UpdatePlaceSuccess = "Cập nhật địa điểm thành công";
    public const string UpdatePlaceFailed = "Cập nhật địa điểm thất bại";
    public const string DeletePlaceSuccess = "Xóa địa điểm thành công";
    public const string DeletePlaceFailed = "Xóa địa điểm thất bại";
    public const string PlaceNotFound = "Địa điểm không tồn tại";

    // Feedback
    public const string FeedbackAdded = "Gửi phản hồi thành công";
    public const string FeedbackNotAdded = "Gửi phản hồi thất bại";
    public const string FeedbackNotFound = "Phản hồi không tồn tại";
    public const string FeedbackUpdated = "Cập nhật phản hồi thành công";
    public const string FeedbackNotUpdated = "Cập nhật phản hồi thất bại";
    public const string FeedbackDeleted = "Xóa phản hồi thành công";
    public const string FeedbackNotDeleted = "Xóa phản hồi thất bại";
    public const string FeedbackResponded = "Phản hồi đã được trả lời";
    public const string FeedbackNotResponded = "Phản hồi chưa được trả lời";
    public const string FeedbackTimeOut = "Không thể chỉnh sửa phản hồi sau 15 ngày";
    public const string FeedbackAlreadyExists = "Bạn đã gửi đánh giá rồi";

    // Profile
    public const string UpdateProfileSuccess = "Cập nhật thông tin thành công";
    public const string UpdateProfileFailed = "Cập nhật thông tin thất bại";
    public const string UpdateImageSuccess = "Cập nhật ảnh thành công";
    public const string UpdateImageFailed = "Cập nhật ảnh thất bại";
    public const string UpdateStatusSuccess = "Cập nhật trạng thái tài khoản thành công";
    public const string UpdateStatusFailed = "Cập nhật trạng thái tài khoản thất bại";
    public const string UpdateLocationSuccess = "Cập nhật toạ độ thành công";
    public const string UpdateLocationFailed = "Cập nhật toạ độ thất bại";

    // User & Business
    public const string UserNotFound = "Người dùng không tồn tại";
    public const string BusinessNotFound = "Doanh nghiệp không tồn tại";

    // Token
    public const string InvalidRefreshToken = "Refresh token không hợp lệ";
    public const string ValidRefreshToken = "Refresh token hợp lệ";
    public const string StoreRefreshTokenFailed = "Lưu RefreshToken không thành công";

    // Messages
    public const string MessagesNotFound = "Không tìm thấy tin nhắn";

    // Friendship
    public const string FriendshipAlreadyExists = "Bạn đã kết bạn với người này";
    public const string FriendshipNotFound = "Chưa phải bạn bè của nhau";
    public const string FriendshipRequestSent = "Yêu cầu kết bạn đã được gửi";
    public const string FriendshipRequestFailed = "Gửi yêu cầu kết bạn thất bại";
    public const string FriendshipRemoved = "Đã xóa mối quan hệ bạn bè";
    public const string FriendshipRemoveFailed = "Xóa mối quan hệ bạn bè thất bại";
    public const string FriendshipAccepted = "Đã chấp nhận yêu cầu kết bạn";
    public const string FriendshipAcceptFailed = "Chấp nhận yêu cầu kết bạn thất bại";
    public const string FriendshipDeclined = "Đã từ chối yêu cầu kết bạn";
    public const string FriendshipDeclineFailed = "Từ chối yêu cầu kết bạn thất bại";
    public const string FriendshipRequestCancelled = "Đã hủy yêu cầu kết bạn";
    public const string FriendshipCancelFailed = "Hủy yêu cầu kết bạn thất bại";

    // Specific Not Found Messages
    public const string CityNotFound = "Thành phố không tồn tại";
    public const string CategoryNotFound = "Danh mục không tồn tại";

    // Image
    public const string ImageSaveFailed = "Lưu ảnh thất bại";
    public const string NoImagesProvided = "Không có ảnh nào";
    public const string ImageLimitExceeded = "Vượt quá số lượng hình ảnh";
}
