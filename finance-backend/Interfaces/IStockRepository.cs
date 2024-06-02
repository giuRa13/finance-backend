using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using finance_backend.DTOs.Stock;
using finance_backend.Models;

namespace finance_backend.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync();

        Task <Stock?> GetByIdAsync(int id);

        Task <Stock> CreateAsync(Stock stockModel);

        Task <Stock?> UpdateAsync(int id, UpdateStockRequestDTO stockDTO);

        Task <Stock> DeleteAsync(int id);
    }
}