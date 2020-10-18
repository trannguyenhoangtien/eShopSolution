﻿using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
    public class ResponseResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T ResultObj { get; set; }
    }
}