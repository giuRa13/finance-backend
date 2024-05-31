using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using finance_backend.DTOs.StockDTO;
using finance_backend.Models;

namespace finance_backend.Mappers
{
    public static class StockMapper
    {
        public static StockDTO ToStockDTO(this Stock stockModel)
        {
            return new StockDTO
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                Marketcap = stockModel.Marketcap,
            };
        }
    }
}