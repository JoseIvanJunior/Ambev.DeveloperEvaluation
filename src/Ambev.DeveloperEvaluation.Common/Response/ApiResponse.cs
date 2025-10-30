using System;

namespace Ambev.DeveloperEvaluation.Common.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; init; }
        public string? Message { get; init; }
        public T? Data { get; init; }
    }

    public static class ApiResponse
    {
        public static ApiResponse<T> Success<T>(T data, string? message = null)
            => new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };

        public static ApiResponse<T> Fail<T>(string? message = null)
            => new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
    }
}
