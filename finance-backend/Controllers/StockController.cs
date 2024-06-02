using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using finance_backend.Data;
using finance_backend.DTOs.Stock;
using finance_backend.Helpers;
using finance_backend.Interfaces;
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
        private readonly IStockRepository _stockRepository;
        public StockController(ApplicationDbContext context, IStockRepository stockRepository)
        {
            _context = context;
            _stockRepository = stockRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]QueryObject query)
        {
            if (!ModelState.IsValid) // for validation (annotation)
                return BadRequest(ModelState);

            var stocks = await _stockRepository.GetAllAsync(query); 
            var stocksDTO = stocks.Select(s => s.ToStockDTO());

            return Ok(stocksDTO);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            if (!ModelState.IsValid) // for validation (annotation)
                return BadRequest(ModelState);

            var stock = await _stockRepository.GetByIdAsync(id);

            if (stock == null)
                return NotFound();

            return Ok(stock.ToStockDTO());
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateStockRequestDTO stockDTO)
        {
            if (!ModelState.IsValid) // for validation (annotation)
                return BadRequest(ModelState);

            var stockModel = stockDTO.ToStockFromCreateDTO();
            await _stockRepository.CreateAsync(stockModel);

            return CreatedAtAction(
                nameof(GetById), 
                new{Id = stockModel.Id}, 
                stockModel.ToStockDTO() //(201)
            );
        }


        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO updatedDTO)
        {
            if (!ModelState.IsValid) // for validation (annotation)
                return BadRequest(ModelState);

            var stockModel = await _stockRepository.UpdateAsync(id, updatedDTO);

            if (stockModel == null)
                return NotFound();

            return Ok(stockModel.ToStockDTO());
        }


        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) // for validation (annotation)
                return BadRequest(ModelState);

            var stockModel = await _stockRepository.DeleteAsync(id);

            if (stockModel == null)
                return NotFound();

            return Ok(stockModel); 
            // return NoContent
            // (with a Delete NOContentent is a success (204))
        }
    }
}