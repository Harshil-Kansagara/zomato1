using System;
using System.Collections.Generic;
using System.Text;

namespace Zomato.DomainModel.Models
{
    public class RestaurantCollection
    {
        public Restaurant Restaurant { get; set; }
        public List<string> RestaurantLocation { get; set; }
        public List<string> Cuisines { get; set; }
        public List<string> Categories { get; set; }
        public double RatingAvg { get; set; }

        public RestaurantCollection()
        {
            Restaurant = new Restaurant();
            RestaurantLocation = new List<string>();
            Cuisines = new List<string>();
            Categories = new List<string>();
        }
    }
}
