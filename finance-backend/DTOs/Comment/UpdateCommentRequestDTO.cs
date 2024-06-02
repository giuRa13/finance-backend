using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace finance_backend.DTOs.Comment
{
    public class UpdateCommentRequestDTO
    {
        [Required]
        [MinLength(4, ErrorMessage = "Title must be min 4 characters")]
        [MaxLength(50, ErrorMessage = "Title cannot be over 50 characters")]
        public string Title { get; set; } = string.Empty;

        
        [Required]
        [MinLength(5, ErrorMessage = "Content must be min 4 characters")]
        //[MaxLength(500, ErrorMessage = "Content cannot be over 500 characters")]
        public string Content { get; set; } = string.Empty;
    }
}