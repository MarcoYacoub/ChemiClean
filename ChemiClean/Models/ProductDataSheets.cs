using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChemiClean.Models
{
    public class ProductDataSheets
    {
        public ProductDataSheets()
        {

        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public DateTime? LastModified { get; set; }
        public int ProductId { get; set; }
        public virtual TblProduct Product { get; set; }
    }
}
