using ChemiClean.Models;
using ChemiClean.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ChemiCleanTest
{

    public class ProductRepository
    {
        // Update Connection string to your DB.
        private readonly string connectionString = @"Server=localhost\SQLEXPRESS;Database=ChemiCleanTest;Trusted_Connection=True;";
        [Fact]
        public async System.Threading.Tasks.Task GetProductsWithNoFiltrationAsync()
        {
            var options = new DbContextOptionsBuilder<ChemiCleanContext>()
                .UseSqlServer(connectionString)
                .Options;
            var dbContext = new ChemiCleanContext(options);
            
            ProductsRepository productsRepository = new ProductsRepository(dbContext);
            List<TblProduct> products = productsRepository.GetProducts();
            int x = await dbContext.TblProduct.CountAsync();
            Assert.Equal(products.Count, x);
        }
        [Fact]
        public async System.Threading.Tasks.Task GetProductsWithFiltration_PageSizeButNoPageNumberAsync()
        {
            var options = new DbContextOptionsBuilder<ChemiCleanContext>()
                .UseSqlServer(connectionString)
                .Options;
            var dbContext = new ChemiCleanContext(options);

            ProductsRepository productsRepository = new ProductsRepository(dbContext);
            List<TblProduct> products = productsRepository.GetProducts("",null,20);
            int x = await dbContext.TblProduct.CountAsync();
            Assert.Equal(products.Count, x);
        }
        [Fact]
        public void GetProductsWithFiltration_CaseSensitive()
        {
            var options = new DbContextOptionsBuilder<ChemiCleanContext>()
                .UseSqlServer(connectionString)
                .Options;
            var dbContext = new ChemiCleanContext(options);

            ProductsRepository productsRepository = new ProductsRepository(dbContext);
            List<TblProduct> products_upper = productsRepository.GetProducts("A");
            List<TblProduct> products_lower = productsRepository.GetProducts("a");
            Assert.Equal(products_upper, products_lower);
        }
        [Fact]
        public void GetProductsWithFiltration_PageNumberVeryBigNumber()
        {
            var options = new DbContextOptionsBuilder<ChemiCleanContext>()
                .UseSqlServer(connectionString)
                .Options;
            var dbContext = new ChemiCleanContext(options);

            ProductsRepository productsRepository = new ProductsRepository(dbContext);
            List<TblProduct> products = productsRepository.GetProducts("",1000,1000,"");
            Assert.NotNull(products);
        }
        [Fact]
        public void GetProductsWithFiltration_KeyWordEmptyString()
        {
            var options = new DbContextOptionsBuilder<ChemiCleanContext>()
                .UseSqlServer(connectionString)
                .Options;
            var dbContext = new ChemiCleanContext(options);

            ProductsRepository productsRepository = new ProductsRepository(dbContext);
            List<TblProduct> productsEmpty = productsRepository.GetProducts("");
            List<TblProduct> productsAll = productsRepository.GetProducts();
            Assert.Equal(productsEmpty, productsAll);
        }
        [Fact]
        public void GetProductDocumentsLastModified_NonExistingIds()
        {
            var options = new DbContextOptionsBuilder<ChemiCleanContext>()
                .UseSqlServer(connectionString)
                .Options;
            var dbContext = new ChemiCleanContext(options);

            ProductsRepository productsRepository = new ProductsRepository(dbContext);
            
            List<TblProduct> productsAll = productsRepository.GetProducts().OrderBy(p=>p.Id).ToList();
            var productDocumentAll = productsRepository.GetProductDocumentsLastModified(productsAll.Select(p=>p.Id).ToList());
            var fakeIds=new List<int> { productDocumentAll.Last().Key, productsAll.Last().Id+1,productsAll.Last().Id+2 };
            var productDocument=productsRepository.GetProductDocumentsLastModified(fakeIds);
            Assert.Equal(1, productDocument.Count);
        }
    }
}
