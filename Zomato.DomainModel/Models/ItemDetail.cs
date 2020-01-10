using System;
using System.Collections.Generic;
using System.Text;

namespace Zomato.DomainModel.Models
{
    public class ItemDetail
    {
        public int OrderId { get; set; }
        public string ItemName { get; set; }
        public int ItemQuantity { get; set; }
        public int ItemPrice { get; set; }

        public ItemDetail(int orderId, string itemName, int itemQuantity, int itemPrice)
        {
            OrderId = orderId;
            ItemName = itemName;
            ItemQuantity = itemQuantity;
            ItemPrice = itemPrice;
        }
    }
}
