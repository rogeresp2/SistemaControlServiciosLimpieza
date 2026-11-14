using System.Collections.Generic;
using System.Text;

namespace SistemaControlServiciosLimpieza.Application.Dtos
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public int StatusCode { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, int statusCode = 200, string message = "")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                StatusCode = statusCode,
                Data = data
            };
        }

        public static ApiResponse<T> SuccessResponse()
        {
            return new ApiResponse<T>
            {
                Success = true,
                StatusCode = 200
            };
        }
        public static ApiResponse<T> ErrorResponse(string message, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                StatusCode = statusCode
            };
        }
    }
}
