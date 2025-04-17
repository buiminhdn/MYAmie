namespace Common.Responses;

public class ApiResponse
{
    public bool IsSuccess { get; }
    public string Message { get; }
    public object Data { get; }

    protected ApiResponse(bool isSuccess, string message, object data)
    {
        IsSuccess = isSuccess;
        Message = message;
        Data = data;
    }

    public static ApiResponse Success(string message = "Success", object data = null)
    {
        return new ApiResponse(true, message, data);
    }

    public static ApiResponse Failure(string message = "Failed", object data = null)
    {
        return new ApiResponse(false, message, data);
    }
}

public class ApiResponse<T> : ApiResponse where T : class
{
    public new T Data { get; }

    private ApiResponse(bool isSuccess, string message, T data)
        : base(isSuccess, message, data)
    {
        Data = data;
    }

    public static ApiResponse<T> Success(T data = null, string message = "Success")
    {
        return new ApiResponse<T>(true, message, data);
    }

    public static ApiResponse<T> Failure(string message)
    {
        return new ApiResponse<T>(false, message, null);
    }
}
