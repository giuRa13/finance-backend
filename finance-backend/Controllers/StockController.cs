using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using finance_backend.Data;
using finance_backend.DTOs.StockDTO;
using finance_backend.Mappers;
using finance_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace finance_backend.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _context.Stocks.ToListAsync();
            var stocksDTO = stocks.Select(s => s.ToStockDTO());

            return Ok(stocksDTO);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            var stock = await _context.Stocks.FindAsync(id);

            if (stock == null)
                return NotFound();

            return Ok(stock.ToStockDTO());
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateStockRequestDTO stockDTO)
        {
            var stockModel = stockDTO.ToStockFromCreateDTO();
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(
                nameof(GetById), 
                new{Id = stockModel.Id}, 
                stockModel.ToStockDTO() //(201)
            );
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO updatedDTO)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if (stockModel == null)
                return NotFound();

            stockModel.Symbol = updatedDTO.Symbol;
            stockModel.CompanyName = updatedDTO.CompanyName;
            stockModel.Purchase = updatedDTO.Purchase;
            stockModel.LastDiv = updatedDTO.LastDiv;
            stockModel.Industry = updatedDTO.Industry;
            stockModel.Marketcap = updatedDTO.Marketcap;

            await _context.SaveChangesAsync();

            return Ok(stockModel.ToStockDTO());
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if (stockModel == null)
                return NotFound();

            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();

            return Ok(stockModel); 
            // return NoContent
            // (with a Delete NOContentent is a success (204))
        }
    }
}