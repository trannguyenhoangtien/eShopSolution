using eShopSolution.Data.EF;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Languages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly IConfiguration _configurable;
        private readonly EShopDbContext _context;
        public LanguageService(IConfiguration configuration, EShopDbContext context)
        {
            _configurable = configuration;
            _context = context;
        }
        public Task<ResponseSuccessResult<List<LanguageVm>>> GetAll()
        {
            var languages = _context.Languages.Select(x => new LanguageVm()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            return new ResponseSuccessResult<List<LanguageVm>>(languages);
        }
    }
}
