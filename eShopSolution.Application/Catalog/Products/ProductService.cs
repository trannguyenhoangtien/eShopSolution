using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using eShopSolution.Application.Common;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Catalog.Categories;

namespace eShopSolution.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;

        public ProductService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                isDefault = request.IsDefault,
                ProductId = productId,
                SortOrder = request.SortOrder,
            };

            //Save Image
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }

            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;
        }

        public async Task<int> AddImages(int productId, List<IFormFile> files)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product: {productId}");

            foreach (var file in files)
            {
                var productImage = new ProductImage()
                {
                    DateCreated = DateTime.Now,
                    FileSize = file.Length,
                    ImagePath = await SaveFile(file)
                };
                _context.ProductImages.Add(productImage);
            }

            return await _context.SaveChangesAsync();
        }

        public async Task AddViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<ResponseResult<bool>> CategoryAssign(int id, CategoryAssignRequest request)
        {
            try
            {


                var user = await _context.Products.FindAsync(id);
                if (user == null)
                    return new ResponseErrorResult<bool>("Sản phẩm không tồn tại");

                var removeCategories = request.Categories.Where(x => x.Selected == false).ToList();
                if (removeCategories.Count > 0)
                {
                    var cateIds = removeCategories.Select(x => x.Id).ToList();
                    var productInCategory = await _context.ProductInCategories
                        .Where(x => cateIds.Contains(x.CategoryId.ToString()) && x.ProductId == id)
                        .ToListAsync();
                    if (productInCategory.Count > 0)
                    {
                        _context.ProductInCategories.RemoveRange(productInCategory);
                    }
                }

                var addCategories = request.Categories.Where(x => x.Selected).ToList();
                foreach (var category in addCategories)
                {
                    var cateIds = addCategories.Select(x => x.Id).ToList();
                    var productInCategory = await _context.ProductInCategories
                        .Where(x => cateIds.Contains(x.CategoryId.ToString()) && x.ProductId == id)
                        .ToListAsync();
                    if (productInCategory.Count > 0)
                    {
                        await _context.ProductInCategories.AddRangeAsync(productInCategory);
                    }
                }

                await _context.SaveChangesAsync();

                return new ResponseSuccessResult<bool>();
            }
            catch (Exception ex)
            {
                return new ResponseErrorResult<bool>(ex.InnerException.Message);
            }
        }

        public async Task<ResponseResult<int>> Create(ProductCreateRequest request)
        {
            try
            {
                var product = new Product()
                {
                    Price = request.Price,
                    OriginalPrice = request.OriginalPrice,
                    Stock = request.Stock,
                    ViewCount = 0,
                    DateCreated = DateTime.Now,
                    ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId,
                    }
                }
                };

                //Save Image
                if (request.ThumbnailImage != null)
                {
                    product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail Image",
                        DateCreated = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await SaveFile(request.ThumbnailImage),
                        isDefault = true,
                        SortOrder = 1
                    }
                };
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return new ResponseSuccessResult<int>(product.Id);
            }
            catch (Exception ex)
            {
                return new ResponseErrorResult<int>(ex.InnerException.Message);
            }
        }

        public async Task<ResponseResult<int>> Delete(int productId)
        {
            try
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null) throw new EShopException($"Cannot find a product: {productId}");

                //Delete Image
                var images = await _context.ProductImages.Where(x => x.ProductId == productId).ToListAsync();
                foreach (var image in images)
                {
                    await _storageService.DeleteFileAsync(image.ImagePath);
                }

                _context.Products.Remove(product);
                return new ResponseSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ResponseErrorResult<int>(ex.InnerException.Message);
            }
        }

        public async Task<PagedResult<ProductVm>> GetAllByCategoryId(GetClientProductPagingRequest request, string languageId)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, pic };

            if (request.CategoryId.HasValue && request.CategoryId > 0)
            {
                query = query.Where(p => request.CategoryId == request.CategoryId);
            }

            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ProductVm()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();

            var pagedResult = new PagedResult<ProductVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };

            return pagedResult;
        }

        public async Task<PagedResult<ProductVm>> GetAllPaging(GetProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into cpic
                        from c in cpic.DefaultIfEmpty()
                        where pt.LanguageId == request.LanguageId
                        select new { p, pt, pic };

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));

            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(p => p.pic.CategoryId == request.CategoryId);
            }

            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ProductVm()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();

            var pagedResult = new PagedResult<ProductVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };

            return pagedResult;
        }

        public async Task<ResponseResult<ProductVm>> GetById(int productId, string languageId)
        {
            var categories = await (from pic in _context.ProductInCategories
                                    join c in _context.Categories on pic.CategoryId equals c.Id
                                    join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                                    where pic.ProductId == productId && ct.LanguageId == languageId
                                    select new CategoryVm()
                                    {
                                        Id = pic.CategoryId,
                                        Name = ct.Name
                                    }).ToListAsync();

            var data = await (from p in _context.Products
                              join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                              where p.Id == productId && pt.LanguageId == languageId
                              select new ProductVm()
                              {
                                  DateCreated = p.DateCreated,
                                  Description = pt.Description,
                                  Details = pt.Details,
                                  Id = p.Id,
                                  LanguageId = pt.LanguageId,
                                  Name = pt.Name,
                                  OriginalPrice = p.OriginalPrice,
                                  Price = p.Price,
                                  SeoAlias = pt.SeoAlias,
                                  SeoDescription = pt.SeoDescription,
                                  SeoTitle = pt.SeoTitle,
                                  Stock = p.Stock,
                                  ViewCount = p.ViewCount,
                                  Categories = categories
                              }).FirstOrDefaultAsync();

            return new ResponseSuccessResult<ProductVm>(data);
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new EShopException($"Cannot find an Image with imageId: {imageId}");

            var viewModel = new ProductImageViewModel()
            {
                Caption = productImage.Caption,
                DateCreated = productImage.DateCreated,
                ImagePath = productImage.ImagePath,
                ProductId = productImage.ProductId,
                SortOrder = productImage.SortOrder,
                FileSize = productImage.FileSize,
                Id = productImage.Id,
                IsDefault = productImage.isDefault
            };

            return viewModel;
        }

        public async Task<List<ProductImageViewModel>> GetListImage(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product: {productId}");

            return await _context.ProductImages.Where(x => x.ProductId == productId)
                .Select(x => new ProductImageViewModel()
                {
                    Caption = x.Caption,
                    DateCreated = x.DateCreated,
                    ImagePath = x.ImagePath,
                    ProductId = x.ProductId,
                    SortOrder = x.SortOrder,
                    FileSize = x.FileSize,
                    Id = x.Id,
                    IsDefault = x.isDefault
                }).ToListAsync();
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new EShopException($"Cannot find an image with id: {imageId}");

            _context.ProductImages.Remove(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<ResponseResult<int>> Update(ProductUpdateRequest request)
        {
            try
            {
                var product = await _context.Products.FindAsync(request.Id);
                var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id
                    && x.LanguageId == request.LanguageId);
                if (product == null || productTranslation == null) throw new EShopException($"Cannot find a product with id: {request.Id}");

                productTranslation.Name = request.Name;
                productTranslation.Description = request.Description;
                productTranslation.Details = request.Details;
                productTranslation.SeoAlias = request.SeoAlias;
                productTranslation.SeoDescription = request.SeoDescription;
                productTranslation.SeoTitle = request.SeoTitle;

                //Save Image
                if (request.ThumbnailImage != null)
                {
                    var thumnailImage = await _context.ProductImages
                        .FirstOrDefaultAsync(i => i.isDefault == true && i.ProductId == request.Id);
                    if (thumnailImage != null)
                    {
                        thumnailImage.DateCreated = DateTime.Now;
                        thumnailImage.FileSize = request.ThumbnailImage.Length;
                        thumnailImage.ImagePath = await SaveFile(request.ThumbnailImage);
                        _context.ProductImages.Update(thumnailImage);
                    }
                }

                return new ResponseSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ResponseErrorResult<int>(ex.InnerException.Message);
            }
        }

        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new EShopException($"Cannot find an image with id: {imageId}");

            //Save Image
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }

            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product with id: {productId}");

            product.Price = newPrice;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int qty)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product with id: {productId}");

            product.Stock += qty;
            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
    }
}