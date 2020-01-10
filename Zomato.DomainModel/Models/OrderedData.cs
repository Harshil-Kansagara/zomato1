using System;
using System.Collections.Generic;
using System.Text;

namespace Zomato.DomainModel.Models
{
    public class OrderedData
    {
        public string UserId { get; set; }
        public int AddressId { get; set; }
        public string RestaurantName { get; set; }
        public List<ItemDataCollection> Items { get; set; }
    }
}
