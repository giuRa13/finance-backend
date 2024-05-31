using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using finance_backend.Data;
using finance_backend.DTOs.StockDTO;
using finance_backend.Mappers;
using finance_backend.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAll()
        {
            var stocks = _context.Stocks.ToList()
                .Select(s => s.ToStockDTO());

            return Ok(stocks);
        }


        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute]int id)
        {
            var stock = _context.Stocks.Find(id);

            if (stock == null)
                return NotFound();

            return Ok(stock.ToStockDTO());
        }


        [HttpPost]
        public IActionResult Create([FromBody]CreateStockRequestDTO stockDTO)
        {
            var stockModel = stockDTO.ToStockFromCreateDTO();
            _context.Stocks.Add(stockModel);
            _context.SaveChanges();
            return CreatedAtAction(
                nameof(GetById), 
                new{Id = stockModel.Id}, 
                stockModel.ToStockDTO() //(201)
            );
        }


        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO updatedDTO)
        {
            var stockModel = _context.Stocks.FirstOrDefault(x => x.Id == id);

            if (stockModel == null)
                return NotFound();

            stockModel.Symbol = updatedDTO.Symbol;
            stockModel.CompanyName = updatedDTO.CompanyName;
            stockModel.Purchase = updatedDTO.Purchase;
            stockModel.LastDiv = updatedDTO.LastDiv;
            stockModel.Industry = updatedDTO.Industry;
            stockModel.Marketcap = updatedDTO.Marketcap;

            _context.SaveChanges();

            return Ok(stockModel.ToStockDTO());
        }


        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var stockModel = _context.Stocks.FirstOrDefault(x => x.Id == id);

            if (stockModel == null)
                return NotFound();

            _context.Stocks.Remove(stockModel);
            _context.SaveChanges();

            return Ok(stockModel); 
            // return NoContent
            // (with a Delete NOContentent is a success (204))
        }
    }
}