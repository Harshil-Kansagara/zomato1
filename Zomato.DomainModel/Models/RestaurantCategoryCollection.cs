using System;
using System.Collections.Generic;
using System.Text;

namespace Zomato.DomainModel.Models
{
    public class RestaurantCategoryCollection
    {
        public RestaurantCategoryCollection(int restaurantId, string categoryName)
        {
            RestaurantId = restaurantId;
            CategoryName = categoryName;
        }

        public int RestaurantId { get; set; }
        public string CategoryName { get; set; }
    }
}
