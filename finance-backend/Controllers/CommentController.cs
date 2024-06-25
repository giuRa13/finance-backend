using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using finance_backend.DTOs.Comment;
using finance_backend.Interfaces;
using finance_backend.Mappers;
using finance_backend.Models;
using finance_backend.Repository;
using finance_backend.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using finance_backend.Helpers;

namespace finance_backend.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController :ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFMPService _fmpService;
        public CommentController(ICommentRepository commentRepository, 
        IStockRepository stockRepository, 
        UserManager<AppUser> userManager,
        IFMPService fMPService)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
            _userManager = userManager;
            _fmpService = fMPService;
        }



        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery]CommentQueryObject queryObject)
        {
            if (!ModelState.IsValid) // for validation (annotation)
                return BadRequest(ModelState);

            var comments = await _commentRepository.GetAllAsync(queryObject);
            var commentDTO = comments.Select(s => s.ToCommentDTO());

            return Ok(commentDTO);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var comment = await _commentRepository.GetByIdAsync(id);

            if (comment == null)
                return NotFound();

            return Ok(comment.ToCommentDTO());
        }


        [HttpPost]
        [Route("{symbol:alpha}")]
        public async Task<IActionResult> Create([FromRoute]string symbol, CreateCommentRequestDTO commentDTO)
        {
            if (!ModelState.IsValid) // for validation (annotation)
                return BadRequest(ModelState);

            var stock = await _stockRepository.GetBySymbolAsync(symbol);

            if (stock == null)
            {
                stock = await _fmpService.FindStockBySymbolAsync(symbol);
                if(stock == null)
                {
                    return BadRequest("Stock not found!");
                }
                else
                {
                    await _stockRepository.CreateAsync(stock);
                }
            }

            var username = User.GetUsername();  
            var appUser = await _userManager.FindByNameAsync(username);  

            var commentModel = commentDTO.ToCommentFromCreate(stock.Id);
            commentModel.AppUserId = appUser.Id;    
            await _commentRepository.CreateAsync(commentModel);
            return CreatedAtAction(
                nameof(GetById),
                new { id = commentModel.Id},
                commentModel.ToCommentDTO() );
        }


        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) // for validation (annotation)
                return BadRequest(ModelState);

            var commentModel = await _commentRepository.DeleteAsync(id);

            if (commentModel == null)
                return NotFound("Comment does not exists!");

            return Ok(commentModel);
        }


        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody]UpdateCommentRequestDTO commentDTO)
        {
            var comment = await _commentRepository.UpdateAsync(id, commentDTO.ToCommentFromUpdate());

            if (comment == null)
                return NotFound("Comment not found!");

            return Ok(comment.ToCommentDTO());
        }
    }
}