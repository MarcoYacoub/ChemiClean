using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChemiClean.Models;

namespace ChemiClean.Repositories
{
    public class ProductsRepository : BaseRepository,IProductsRepository
    {
        public ProductsRepository(ChemiCleanContext dbContext)
            : base(dbContext)
        {

        }

        public void AddProuctDataSheet(ProductDataSheets productSavedFile)
        {
            _dbContext.ProductDataSheets.Add(productSavedFile);
            _dbContext.SaveChanges();
        }

        public TblProduct getProductbyId(int productId)
        {
            return _dbContext.TblProduct.FirstOrDefault(p => p.Id == productId);
        }

        public List<TblProduct> GetProducts(string keyword = null, int? pageNumber = null, int? pageSize = null, string SupplierName = null)
        {
            var products = _dbContext.TblProduct.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                products = products.Where(p => p.ProductName.ToLower().Contains(keyword.ToLower()));
            }
            if (!string.IsNullOrEmpty(SupplierName))
            {
                products = products.Where(p => p.SupplierName.ToLower().Contains(SupplierName.ToLower()));
            }
            if (pageSize.HasValue && pageNumber.HasValue)
            {
                products = products.Skip(pageSize.Value * (pageNumber.Value - 1)).Take(pageSize.Value);
            }
            
            return products.ToList();
        }

        public int GetProductsCount(string keyword = null, string SupplierName = null)
        {
            var products = _dbContext.TblProduct.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                products = products.Where(p => p.ProductName.ToLower().Contains(keyword.ToLower()));
            }
            if (!string.IsNullOrEmpty(SupplierName))
            {
                products = products.Where(p => p.SupplierName.ToLower().Contains(SupplierName.ToLower()));
            }
            return products.Count();
        }

        public ProductDataSheets GetProuctDataSheet(int productId)
        {
            return _dbContext.ProductDataSheets.FirstOrDefault(pd => pd.ProductId == productId);
        }

        public List<string> GetSuppliersDistinct()
        {
            return _dbContext.TblProduct.Select(p => p.SupplierName).Distinct().ToList();
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public Dictionary<int,DateTime?> GetProductDocumentsLastModified(List<int> determinedProductIds)
        {
            var ProductDocumentsLastModified = _dbContext.TblProduct.Where(p => determinedProductIds.Contains(p.Id))
                .Join(_dbContext.ProductDataSheets, p => p.Id, pd => pd.ProductId, (p, pd) => new { p.Id, pd.LastModified }).ToDictionary(plm => plm.Id,plm=>plm.LastModified);
            return ProductDocumentsLastModified;
        }
    }
}
