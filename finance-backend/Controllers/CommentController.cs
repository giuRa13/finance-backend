using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepository.GetAllAsync();
            var commentDTO = comments.Select(s => s.ToCommentDTO());

            return Ok(commentDTO);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);

            if (comment == null)
                return NotFound();

            return Ok(comment.ToCommentDTO());
        }
    }
}