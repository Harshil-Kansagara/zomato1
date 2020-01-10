using System;
using System.Collections.Generic;
using System.Text;

namespace Zomato.DomainModel.Models
{
    public class CommentCollection
    {
        public int CommentId { get; set; }
        public string CommentData { get; set; }
        public string UserName { get; set; }
    }
}
