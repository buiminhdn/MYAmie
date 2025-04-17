export const handleError = (error: any): string => {
  // Ignore "Network Error" and return a custom message
  if (error?.message === "Network Error") {
    return "Không thể kết nối đến máy chủ. Vui lòng kiểm tra kết nối mạng.";
  }

  // Handle ApiResponse format
  if (error?.response?.data?.message) {
    return error.response.data.message;
  }

  if (error?.response?.data) {
    return error.response.data;
  }

  // Handle direct message
  if (error?.message) {
    return error.message;
  }

  // Fallback message
  return "Đã xảy ra lỗi không mong muốn";
};
