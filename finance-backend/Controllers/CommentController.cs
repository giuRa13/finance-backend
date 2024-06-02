using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using finance_backend.DTOs.Comment;
using finance_backend.Interfaces;
using finance_backend.Mappers;
using finance_backend.Repository;
using Microsoft.AspNetCore.Mvc;

namespace finance_backend.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController :ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid) // for validation (annotation)
                return BadRequest(ModelState);

            var comments = await _commentRepository.GetAllAsync();
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


        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute]int stockId, CreateCommentRequestDTO commentDTO)
        {
            if (!ModelState.IsValid) // for validation (annotation)
                return BadRequest(ModelState);

            if (!await _stockRepository.StockExists(stockId))
            {
                return BadRequest("Stock does not exists!");
            }

            var commentModel = commentDTO.ToCommentFromCreate(stockId);
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