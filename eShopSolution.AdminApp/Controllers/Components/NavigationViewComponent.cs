﻿using eShopSolution.AdminApp.Models;
using eShopSolution.AdminApp.Services;
using eShopSolution.Utilities.Constaints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Controllers.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly ILanguageApiClient _languageApiClient;
        public NavigationViewComponent(ILanguageApiClient languageApiClient)
        {
            _languageApiClient = languageApiClient;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var languages = await _languageApiClient.GetAll();
            var navigationVm = new NavigationViewModel()
            {
                CurrentLanguage = HttpContext.Session.GetString(SystemContains.AppSettings.DefaultLanguageId),
                Languages = languages.ResultObj
            };
            return View("Defatult", navigationVm);
        }
    }
}