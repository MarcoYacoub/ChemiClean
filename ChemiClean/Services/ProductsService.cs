using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using ChemiClean.Models;
using ChemiClean.Repositories;
using ChemiClean.ViewModels;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Serilog;

namespace ChemiClean.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _productsRepository;
        public ProductsService(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public  async Task<FileViewModel> DownloadFile(int ProductId)
        {
            var product = _productsRepository.getProductbyId(ProductId);
            if (product!=null)
            {
                var productSavedFile = _productsRepository.GetProuctDataSheet(ProductId);
                try
                {
                    using (var client = new HttpClient())
                    {

                        using (var result = await client.GetAsync(product.Url))
                        {
                            if (result.IsSuccessStatusCode)
                            {
                                var FileInfo = new FileViewModel();
                                FileInfo.content = await result.Content.ReadAsByteArrayAsync();
                                FileInfo.ContentType = result.Content.Headers.ContentType.MediaType;
                                FileInfo.LastModified = result.Content.Headers.LastModified.HasValue ? (DateTime?)result.Content.Headers.LastModified.Value.DateTime : null;

                                if (productSavedFile == null)
                                {
                                    productSavedFile = new ProductDataSheets
                                    {
                                        Content = FileInfo.content,
                                        ContentType = FileInfo.ContentType,
                                        LastModified = FileInfo.LastModified,
                                        ProductId = ProductId,
                                    };
                                    _productsRepository.AddProuctDataSheet(productSavedFile);
                                }
                                else if (productSavedFile.LastModified != FileInfo.LastModified)
                                {
                                    productSavedFile.Content = FileInfo.content;
                                    productSavedFile.ContentType = FileInfo.ContentType;
                                    productSavedFile.LastModified = FileInfo.LastModified;
                                    _productsRepository.Commit();

                                }
                                return FileInfo;
                            }
                            else
                            {
                                product.DocumentAvaliable = false;
                                _productsRepository.Commit();
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    product.DocumentAvaliable = false;
                    _productsRepository.Commit();
                    Log.Error(ex, "Could not get file data");
                }
                if (productSavedFile != null)
                {
                    return new FileViewModel
                    {
                        content = productSavedFile.Content,
                        ContentType = productSavedFile.ContentType,
                        LastModified = productSavedFile.LastModified
                    };
                }
            }
            
            return null;
        }

        public async Task<ProductsViewModel> GetProductsAsync(string keyword = null, int? pageNumber = null, int? pageSize = null , string SupplierName = null)
        {
            var productsVM = new List<Products>();
            var products=_productsRepository.GetProducts(keyword, pageNumber,pageSize, SupplierName);
            var totalCount = _productsRepository.GetProductsCount(keyword, SupplierName);
            productsVM = products.Select(p => new Products {
                Id = p.Id,
                ProductName = p.ProductName,
                SupplierName = p.SupplierName,
                Url = p.Url,
                FileAvaliable = p.DocumentAvaliable.HasValue ? p.DocumentAvaliable.Value : false
            }).ToList();
            var undeterminedProductIds = products.Where(p => !p.DocumentAvaliable.HasValue).Select(p=>p.Id).ToList();
            var determinedProductIds= products.Where(p => p.DocumentAvaliable.HasValue).Select(p => p.Id).ToList();
            var productsDocumentsLastModified=_productsRepository.GetProductDocumentsLastModified(determinedProductIds);
            foreach (var productVM in productsVM)
            {
                var product = products.FirstOrDefault(p => p.Id == productVM.Id);
                if (!product.DocumentAvaliable.HasValue)
                {
                    product.DocumentAvaliable = await GetFileInfo(productVM);
                }
                else if(product.DocumentAvaliable.Value)
                {
                    if (productsDocumentsLastModified.ContainsKey(productVM.Id))
                    {
                        productVM.LastModified = productsDocumentsLastModified[productVM.Id];
                    }
                }
                if (productVM.LastModified.HasValue && (DateTime.UtcNow - productVM.LastModified.Value).TotalDays < 3)
                {
                    productVM.RecentlyModified=true;
                }
            }
            if (undeterminedProductIds.Count>0)
            {
                _productsRepository.Commit();
            }
            return new ProductsViewModel {Products= productsVM,TotalCount=totalCount };


        }
        public async Task<bool> GetFileInfo(Products product)
        {
            product.FileAvaliable = false;
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Head, product.Url);
                try
                {
                    using (var result = await client.SendAsync(request))
                    {
                        
                        if (result.IsSuccessStatusCode && result.Content.Headers.ContentType.MediaType!= "text/html")
                        {
                            product.LastModified= result.Content.Headers.LastModified.HasValue ? (DateTime?)result.Content.Headers.LastModified.Value.DateTime : null; ;
                            product.FileAvaliable = true;
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Could not get file data");
                }
                
            }
            return false;
        }
        public List<string> GetSuppliers()
        {
           return _productsRepository.GetSuppliersDistinct();
        }

        public Products GetProductById(int productId)
        {
            var product= _productsRepository.getProductbyId(productId);
            if (product!=null)
            {
                return new Products {
                    Id=product.Id,
                    ProductName= product.ProductName,
                    SupplierName= product.SupplierName,
                    Url= product.Url
                };
            }
            return null;
        }

        public async Task<bool> CheckProductDataSheetAvailabilityAsync(int productId)
        {
            var product = _productsRepository.getProductbyId(productId);
            if (product!=null)
            {
                var productVm = new Products
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    SupplierName = product.SupplierName,
                    Url = product.Url
                };
                var avaliable = await GetFileInfo(productVm);
                product.DocumentAvaliable = avaliable;
                _productsRepository.Commit();
                return avaliable;
            }
            return false;

        }
    }
}
