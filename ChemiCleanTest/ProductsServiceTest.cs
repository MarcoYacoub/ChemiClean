using ChemiClean.Models;
using ChemiClean.Repositories;
using ChemiClean.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ChemiCleanTest
{
    public class ProductsServiceTest
    {
        // Update Connection string to your DB.
        private readonly string connectionString = @"Server=localhost\SQLEXPRESS;Database=ChemiCleanTest;Trusted_Connection=True;";
        // Should Make sure to close connections to DB.
        // There are a lot of other cases that should be covered.
        [Fact]
        public async System.Threading.Tasks.Task DownLoadFile_NonexitingFile()
        {
            var options = new DbContextOptionsBuilder<ChemiCleanContext>()
                .UseSqlServer(connectionString)
                .Options;
            var dbContext = new ChemiCleanContext(options);

            ProductsRepository productsRepository = new ProductsRepository(dbContext);
            ProductsService productsService = new ProductsService(productsRepository);
            var NonExistingFile=await productsService.DownloadFile(0);
            Assert.Null(NonExistingFile);
        }
        [Fact]
        public async System.Threading.Tasks.Task DownLoadFile_NonexitingURL()
        {
            var options = new DbContextOptionsBuilder<ChemiCleanContext>()
                .UseSqlServer(connectionString)
                .Options;
            var dbContext = new ChemiCleanContext(options);

            ProductsRepository productsRepository = new ProductsRepository(dbContext);
            ProductsService productsService = new ProductsService(productsRepository);
            var product=productsRepository.GetProducts().FirstOrDefault();
            if (product!=null)
            {
                product.Url = "None";
                productsRepository.Commit();
            }
            var NonExistingFile = await productsService.DownloadFile(product.Id);
            Assert.Null(NonExistingFile);
        }
        [Fact]
        public void GetProductById_NoneExistingId()
        {
            var options = new DbContextOptionsBuilder<ChemiCleanContext>()
                .UseSqlServer(connectionString)
                .Options;
            var dbContext = new ChemiCleanContext(options);

            ProductsRepository productsRepository = new ProductsRepository(dbContext);
            ProductsService productsService = new ProductsService(productsRepository);
            
            var NonExistingFile = productsService.GetProductById(0);
            Assert.Null(NonExistingFile);
        }
        [Fact]
        public async System.Threading.Tasks.Task CheckProductDataSheetAvailabilityAsync_NonExistingProduct()
        {
            var options = new DbContextOptionsBuilder<ChemiCleanContext>()
                .UseSqlServer(connectionString)
                .Options;
            var dbContext = new ChemiCleanContext(options);

            ProductsRepository productsRepository = new ProductsRepository(dbContext);
            ProductsService productsService = new ProductsService(productsRepository);

            var NonExistingFile =await productsService.CheckProductDataSheetAvailabilityAsync(0);
            Assert.False(NonExistingFile);
        }
        [Fact]
        public async System.Threading.Tasks.Task CheckProductDataSheetAvailabilityAsync_NonExistingURL()
        {
            var options = new DbContextOptionsBuilder<ChemiCleanContext>()
                .UseSqlServer(connectionString)
                .Options;
            var dbContext = new ChemiCleanContext(options);

            ProductsRepository productsRepository = new ProductsRepository(dbContext);
            ProductsService productsService = new ProductsService(productsRepository);
            var product = productsRepository.GetProducts().FirstOrDefault();
            if (product != null)
            {
                product.Url = "None";
                productsRepository.Commit();
            }
            var NonExistingFile = await productsService.CheckProductDataSheetAvailabilityAsync(product.Id);
            Assert.False(NonExistingFile);
        }
    }
}
