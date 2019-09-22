﻿using System;
using System.Collections.Generic;

namespace ChemiClean.Models
{
    public partial class TblProduct
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
        public string Url { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? DocumentAvaliable { get; set; }
    }
}
