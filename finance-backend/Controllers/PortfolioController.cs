using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using finance_backend.Extensions;
using finance_backend.Interfaces;
using finance_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace finance_backend.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IFMPService _fmpService;
        
        public PortfolioController(
            UserManager<AppUser> userManager, 
            IStockRepository stockRepository,
            IPortfolioRepository portfolioRepository,
            IFMPService fMPService)
        {
            _stockRepository = stockRepository;
            _userManager = userManager; 
            _portfolioRepository = portfolioRepository;
            _fmpService = fMPService;
        }



        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
            return Ok(userPortfolio);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol) // add stock to a portfolio
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepository.GetBySymbolAsync(symbol);
            
            if (stock == null)
            {
                stock = await _fmpService.FindStockBySymbolAsync(symbol);
                if(stock == null)
                {
                    return BadRequest("Stock not found!");
                }
                else{
                    await _stockRepository.CreateAsync(stock);
                }
            }

            if(stock == null) return BadRequest("Stock not found!");

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

            if(userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())) 
                return BadRequest("Stock already in the portfolio!");

            var portfolioModel = new Portfolio   
            {
                StockId = stock.Id,
                AppUserId = appUser.Id,              
            };

            await _portfolioRepository.CreateAsync(portfolioModel);

            if(portfolioModel == null)
            { 
                return StatusCode(500, "Could not create");
            }
            else 
            {
                return Created();
            }
        }


        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower()  == symbol.ToLower());

            if (filteredStock.Count() == 1)
            {
                await _portfolioRepository.DeletePortfolio(appUser, symbol);
            }
            else
            {
                return BadRequest("Stock not in your Portfolio!");
            }

            return Ok();   
        }        
    }
}