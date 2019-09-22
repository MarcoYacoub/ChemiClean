using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChemiClean.Pages.Shared
{
    public class FileNotFoundModel : PageModel
    {
        public string message { get; set; }
        public void OnGet()
        {
            message = "we could not find the requested file neither online nor in the local database";
        }
    }
}