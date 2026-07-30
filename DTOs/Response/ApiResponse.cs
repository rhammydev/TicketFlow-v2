namespace TicketFlow.DTOs.Response;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    
    public string Message { get; set; }
    
    public T Data { get; set; }


    public static ApiResponse<T> SuccessResponse(T data, string message = "Operation completed successfully")
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> FailureResponse(string message)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Message = message,
            Data = default(T)!
        };
    }
}