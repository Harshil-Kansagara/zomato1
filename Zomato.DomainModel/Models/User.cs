using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Zomato.DomainModel.Models
{
    public class User
    {
        public string UserName { get; set; }

        public string UserMobileNumber { get; set; }

        public string UserEmailAddress { get; set; }

        public string UserPassword { get; set; }

        public string UserRole { get; set; }
    }
}
