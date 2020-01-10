using System;
using System.Collections.Generic;
using System.Text;

namespace Zomato.DomainModel.Models
{
    public class LikeDataCollection
    {
        public int LikeId { get; set; }
        public string UserName { get; set; }

        public LikeDataCollection(int likeId, string userName)
        {
            LikeId = likeId;
            UserName = userName;
        }
    }
}
