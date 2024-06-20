using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace finance_backend.Models
{
    [Table("Comments")]
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


        // ONE TO ONE (migration and update db)
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}

/* now only one user can be associated with an object
    we put the user as a SUB-MODEL inside the Comment model
        
    ONE TO MANY:
        {
            "comment": "this is a comment",
            "user": [  
                {"user":"user"},
                {"user":"user"},
                {"user":"user"},
            ]
        }
        
     ONE TO ONE:
        {
            "comment": "this is a comment",
            "user": {
                "user":"user"
            }
        }
        
*/