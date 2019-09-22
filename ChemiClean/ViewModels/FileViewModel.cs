using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ChemiClean.ViewModels
{
    public class FileViewModel
    {
        public byte[] content { get; set; }
        public string ContentType { get; set; }
        
        public DateTime? LastModified { get; set; }
    }
}
