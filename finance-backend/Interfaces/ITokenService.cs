using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using finance_backend.Models;

namespace finance_backend.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}