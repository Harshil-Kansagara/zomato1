using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Zomato.DomainModel.Models
{
    public class OrderedItem
    {
        [Key]
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }
        //[ForeignKey("OrderId")]
        //public Order Order { get; set; }

        public int ItemId { get; set; }
        //[ForeignKey("ItemId")]
        //public Menu Menu { get; set; }

        public int ItemQuantity { get; set; }

        public static object AsQueryable()
        {
            throw new NotImplementedException();
        }
    }
}
