﻿using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Utilities.Constaints
{
    public class SystemContants
    {
        public const string MainConectionString = "EShopSolutionDb";
        public const string CartSession = "CartSession";
        public class AppSettings
        {
            public const string DefaultLanguageId = "DefaultLanguageId";
            public const string Token = "Token";
            public const string BaseAddress = "BaseAddress";
        }

        public class ProductSettings
        {
            public const int NumberOfFeaturedProduct = 12;
            public const int NumberOfLastestProduct = 6;
        }

        public class ProductContants
        {
            public const string NA = "N/A";
        }
    }
}