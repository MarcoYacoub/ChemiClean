using ChemiClean.Models;
using ChemiClean.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChemiClean.Repositories
{
    public interface IProductsRepository
    {
        List<TblProduct> GetProducts(string keyword=null, int? pageNumber = null, int? pageSize=null, string SupplierName=null);
        int GetProductsCount(string keyword = null, string SupplierName = null);
        List<string> GetSuppliersDistinct();
        TblProduct getProductbyId(int productId);
        ProductDataSheets GetProuctDataSheet(int productId);
        void AddProuctDataSheet(ProductDataSheets productSavedFile);
        void Commit();
        Dictionary<int, DateTime?> GetProductDocumentsLastModified(List<int> determinedProductIds);
    }
}
