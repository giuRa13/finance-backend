using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finance_backend.Helpers
{
    public class CommentQueryObject
    {
        public string Symbol { get; set; }
        public bool IsDescending { get; set; }
    }
}