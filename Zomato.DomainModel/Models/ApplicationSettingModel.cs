using System;
using System.Collections.Generic;
using System.Text;

namespace Zomato.DomainModel.Models
{
    public class ApplicationSettingModel
    {
        public string JWT_Secret { get; set; }
        public string Client_URL { get; set; }
    }
}
