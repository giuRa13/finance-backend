using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using finance_backend.DTOs.Stock;
using finance_backend.Helpers;
using finance_backend.Models;

namespace finance_backend.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);

        Task<Stock?> GetByIdAsync(int id);

        Task<Stock?> GetBySymbolAsync(string symbol);

        Task<Stock> CreateAsync(Stock stockModel);

        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDTO stockDTO);

        Task<Stock> DeleteAsync(int id);

        Task<bool> StockExists(int id); // for Create Comment
    }
}