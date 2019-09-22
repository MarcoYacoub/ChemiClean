using ChemiClean.Models;
using ChemiClean.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChemiClean.Services
{
    public interface IProductsService
    {
        Task<ProductsViewModel> GetProductsAsync (string keyword=null, int? pageNumber = null, int? pageSize=null,string SupplierName=null);
        List<string> GetSuppliers();
        Task<FileViewModel> DownloadFile(int ProductId);
        Products GetProductById(int productId);
        Task<bool> CheckProductDataSheetAvailabilityAsync(int productId);
    }
}
