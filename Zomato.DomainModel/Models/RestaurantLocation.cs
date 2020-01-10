using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Zomato.DomainModel.Models
{
    public class RestaurantLocation
    {
        [Key]
        public int LocationId { get; set; }

        public int RestaurantId { get; set; }
        [ForeignKey("RestaurantId")]
        public Restaurant Restaurant { get; set; }


        public string Location { get; set; }
    }
}
