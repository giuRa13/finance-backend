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
                stockModel.ToStockDTO()
            );
        }
    }
}