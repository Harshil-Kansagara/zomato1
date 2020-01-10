using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Zomato.DomainModel.Models
{
    public class Cuisine
    {
        [Key]
        public int CuisineId { get; set; }
        public string CuisineName { get; set; }
    }
}
