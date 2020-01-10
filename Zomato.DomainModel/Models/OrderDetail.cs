using System;
using System.Collections.Generic;
using System.Text;

namespace Zomato.DomainModel.Models
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public string Date { get; set; }
        public string RestaurantName { get; set; }
        public string UserName { get; set; }
        public string UserNumber { get; set; }
        public string UserEmail { get; set; }
        public string DeliveryLocation { get; set; }
        public int TotalAmount { get; set; }
        public List<ItemDetail> ItemDetail { get; set; }

        public OrderDetail()
        {
            ItemDetail = new List<ItemDetail>();
        }
    }
}
