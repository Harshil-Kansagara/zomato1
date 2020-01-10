using System;
using System.Collections.Generic;
using System.Text;

namespace Zomato.DomainModel.Models
{
    public class MenuCollection
    {
        public string CuisineName { get; set; }
        public List<Menu> Menus { get; set; }
    }
}
