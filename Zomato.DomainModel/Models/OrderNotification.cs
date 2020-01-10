using System;
using System.Collections.Generic;
using System.Text;

namespace Zomato.DomainModel.Models
{
    public class OrderNotification
    {
        public int OrderId { get; set; }
        public string RestaurantName { get; set; }
    }
}
