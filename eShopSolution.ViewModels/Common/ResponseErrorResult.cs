using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
    public class ResponseErrorResult<T> : ResponseResult<T>
    {
        public ResponseErrorResult()
        {
        }

        public ResponseErrorResult(string message)
        {
            IsSuccess = false;
            Message = message;
        }

        public ResponseErrorResult(string[] validationErrors)
        {
            IsSuccess = false;
            ValidationErrors = validationErrors;
        }

        public string[] ValidationErrors { get; set; }
    }
}