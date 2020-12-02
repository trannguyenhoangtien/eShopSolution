using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Utilities.Constaints
{
    public class SystemContains
    {
        public const string MainConectionString = "EShopSolutionDb";

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
    }
}