using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("products")]
    public class Products : DomainEntity<int>
    {
        [Column(TypeName = "nvarchar(250)")]
        public string? name { get; set; }
        public int? category_id { get; set; }
        [ForeignKey("category_id")]
        public Category? Category { get; set; }
        //[Column(TypeName = "varchar(200)")]
        //public string? slug { get; set; }
        //[Column(TypeName = "varchar(200)")]
        //public string? avatar { get; set; }
        [Column(TypeName = "decimal(18,0)")]
        public decimal? price { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal? reduced_price { get; set; }

        //public string? color { get; set; }
        //public int quantity { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? short_desc { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? description { get; set; }
        
        public int? status { get; set; }
    }
}
