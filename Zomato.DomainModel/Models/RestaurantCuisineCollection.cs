using System;
using System.Collections.Generic;
using System.Text;

namespace Zomato.DomainModel.Models
{
    public class RestaurantCuisineCollection
    {
        public RestaurantCuisineCollection(int restaurantId, string cuisineName)
        {
            RestaurantId = restaurantId;
            CuisineName = cuisineName;
        }

        public int RestaurantId { get; set; }
        public string CuisineName { get; set; }
    }
}
