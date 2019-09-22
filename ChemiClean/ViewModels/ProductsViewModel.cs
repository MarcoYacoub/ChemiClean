using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChemiClean.ViewModels
{
    public class ProductsViewModel
    {
        public List<Products> Products { get; set; }
        public int TotalCount { get; set; }
    }
    public class Products
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
        public string Url { get; set; }
        public DateTime? LastModified { get; set; }
        public bool FileAvaliable { get; set; }
        public bool RecentlyModified { get; internal set; }
    }
}
