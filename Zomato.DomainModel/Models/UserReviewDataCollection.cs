using System;
using System.Collections.Generic;
using System.Text;

namespace Zomato.DomainModel.Models
{
    public class UserReviewDataCollection
    {
        public int ReviewId { get; set; }
        public string RestaurantName { get; set; }

        public UserReviewDataCollection(int reviewId, string restaurantName)
        {
            ReviewId = reviewId;
            RestaurantName = restaurantName;
        }
    }
}
