using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaControlServiciosLimpieza.Application.Models
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }


        public static ServiceResult<T> SuccessResponse(T data, string message = "")
        {
            return new ServiceResult<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ServiceResult<T> SuccessResponse()
        {
            return new ServiceResult<T>
            {
                Success = true,
            };
        }
        public static ServiceResult<T> ErrorResponse(string message)
        {
            return new ServiceResult<T>
            {
                Success = false,
                Message = message,
            };
        }
    }
}
