using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChemiClean.Models;
using ChemiClean.Services;
using ChemiClean.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChemiClean.Controllers
{
    
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;
        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }
        [HttpGet]
        [Route("api/Products/GetProducts")]
        public async Task<ActionResult<ProductsViewModel>> GetProductsListAsync(string keyWord=null,int? pageNumber=null,int? pageSize=null,string supplierName=null)
        {
            var allProducts=await _productsService.GetProductsAsync(keyWord, pageNumber, pageSize, supplierName);
            return Ok(allProducts);
        }
        [HttpGet]
        [Route("api/Products/GetSuppliers")]
        public ActionResult<List<string>> GetSuppliers()
        {
            var allProducts = _productsService.GetSuppliers();
            return Ok(allProducts);
        }
        [HttpGet]
        [Route("api/Products/CheckProductDataSheetAvailability")]
        public async Task<ActionResult<bool>> CheckProductDataSheetAvailability(int productId)
        {
            var avaliable = await _productsService.CheckProductDataSheetAvailabilityAsync(productId);
            return Ok(avaliable);
        }
    }
    
}