using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace finance_backend.Models
{
    [Table("Portfolios")]
    public class Portfolio // the JOIN TABLE
    {
        public string AppUserId { get; set; }

        public int StockId { get; set; }

        public AppUser AppUser { get; set; }

        public Stock Stock { get; set; } 
        
    }
}