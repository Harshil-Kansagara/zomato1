using System;
using System.Collections.Generic;
using System.Text;

namespace Zomato.DomainModel.Models
{
    public class NewRestaurant
    {
        public string RestaurantName { get; set; }
        public List<string> Location { get; set; }
        public List<int> CuisineId { get; set; }
        public List<int> CategoryId { get; set; }
    }
}
