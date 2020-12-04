using eShopSolution.ViewModels.System.Languages;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Models
{
    public class NavigationViewModel
    {
        public List<LanguageVm> Languages { get; set; }
        public string CurrentLanguage { get; set; }
        public string ReturnUrl { get; set; }
    }
}