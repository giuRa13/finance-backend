using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using finance_backend.Data;
using finance_backend.DTOs.Stock;
using finance_backend.Interfaces;
using finance_backend.Mappers;
using finance_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace finance_backend.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;
        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks.Include(c => c.Comments).ToListAsync();
        }   


        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
        }
        

        public async Task<Stock?> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }
     

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDTO stockDTO)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if (existingStock == null)
                return null;

            existingStock.Symbol = stockDTO.Symbol;
            existingStock.CompanyName = stockDTO.CompanyName;
            existingStock.Purchase = stockDTO.Purchase;
            existingStock.LastDiv = stockDTO.LastDiv;
            existingStock.Industry = stockDTO.Industry;
            existingStock.Marketcap = stockDTO.Marketcap;

            await _context.SaveChangesAsync();
            return existingStock;
        }
         

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if (stockModel == null)
                return null;

            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();

            return stockModel;
        }


    }
}