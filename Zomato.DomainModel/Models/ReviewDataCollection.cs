using System;
using System.Collections.Generic;
using System.Text;

namespace Zomato.DomainModel.Models
{
    public class ReviewDataCollection
    {
        public int ReviewId { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public string UserName { get; set; }

        public ReviewDataCollection(int reviewId, int likeCount, int commentCount, string userName)
        {
            ReviewId = reviewId;
            LikeCount = likeCount;
            CommentCount = commentCount;
            UserName = userName;
        }
    }
}
