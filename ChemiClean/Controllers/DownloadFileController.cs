using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChemiClean.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChemiClean.Controllers
{
    public class DownloadFileController : Controller
    {
        private readonly IProductsService _productsService;
        
        public DownloadFileController(IProductsService productsService)
        {
            _productsService = productsService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("DownloadFile/DownloadFileURL/{ProductId}")]
        public async Task<ActionResult> DownloadFileURL(int ProductId)
        {
            
            var template = await _productsService.DownloadFile(ProductId);
            if (template!=null)
            {
                return File(template.content, template.ContentType);
            }
            return RedirectToPage("/Shared/FileNotFound");
        }
    }
}