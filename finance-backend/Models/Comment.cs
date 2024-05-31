using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finance_backend.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        
        public int? StockId { get; set; } // KEY
        public Stock? Stock { get; set; } // NAVIGATION PROPERTY
        // let to navigate the other side of relationship 
        // (es.: Stock.CompanyName, Stock.Marketcap)
    }
}