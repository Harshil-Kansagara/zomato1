using System;
using System.Collections.Generic;
using System.Text;

namespace Zomato.DomainModel.Models
{
    public class UserReviewCollection
    {
        public string RestaurantName { get; set; }
        public Review Review { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
