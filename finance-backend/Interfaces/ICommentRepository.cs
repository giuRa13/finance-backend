using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using finance_backend.Models;

namespace finance_backend.Repository
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();

        Task<Comment?> GetByIdAsync(int id);
    }
}