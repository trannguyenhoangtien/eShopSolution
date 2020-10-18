using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
    public class ResponseSuccessResult<T> : ResponseResult<T>
    {
        public ResponseSuccessResult(T resultObj)
        {
            IsSuccess = true;
            ResultObj = resultObj;
        }

        public ResponseSuccessResult()
        {
            IsSuccess = true;
        }
    }
}