using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChemiClean.Models;
using ChemiClean.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChemiClean.Pages
{
    public class ProductsListingModel : PageModel
    {
        private readonly IProductsService _productsService;
        public List<TblProduct> products { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalCount { get; set; }
        public ProductsListingModel(IProductsService productsService)
        {
            _productsService = productsService;
            PageSize = 20;
            PageNumber = 1;
        }
        public void OnGet()
        {
            products = _productsService.GetProducts();
            TotalCount = products.Count;
            products = products.Take(PageSize * PageNumber).ToList();
            
        }
        public void OnPostLoadMoreAsync(int currentPageNumber,int productsTotalCount)
        {
            products = _productsService.GetProducts();
            var keyWord = Request.Form["keyWord"].ToString();
            if (!string.IsNullOrEmpty(keyWord))
            {
                products = _productsService.GetProducts().Where(p => p.ProductName.ToLower().Contains(keyWord.ToLower())).ToList();
                ViewData["keyWord"] = keyWord;


            }
            TotalCount = products.Count;
            PageNumber = currentPageNumber + 1;
            products = products.Take(PageSize * PageNumber).ToList();
        }
        public void OnPostSearchAsync(int currentPageNumber, int productsTotalCount)
        {
            
            var keyWord = Request.Form["keyWord"].ToString();
            ViewData["keyWord"] = keyWord;
            products = _productsService.GetProducts().Where(p => p.ProductName.ToLower().Contains(keyWord.ToLower())).ToList();
            PageNumber = currentPageNumber + 1;
            TotalCount = products.Count;
            products = products.Take(PageSize * PageNumber).ToList();
        }

    }
}