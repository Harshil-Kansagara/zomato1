using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Zomato.DomainModel.Models
{
    public class OrderNotificationData
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }
        
    }
}
