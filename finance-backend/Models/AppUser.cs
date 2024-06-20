using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace finance_backend.Models
{
    public class AppUser : IdentityUser
    {
        public List<Portfolio> Portfolios{ get; set; } = new List<Portfolio>(); //ONE TO MANY
    }
}