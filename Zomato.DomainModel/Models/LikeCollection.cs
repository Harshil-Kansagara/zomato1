using System;
using System.Collections.Generic;
using System.Text;

namespace Zomato.DomainModel.Models
{
    public class LikeCollection
    {
        public List<Like> Like { get; set; }
        public List<LikeDataCollection> LikeDataCollection { get; set; }
    }
}
