using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;

namespace Zomato.Repository.FollowRepository
{
    public interface IFollowRepository
    {
        Task<List<Follow>> GetFollowList();
        Task<List<Follow>> GetFollowingByUserId(string userId);
        Task<List<Follow>> GetFollowerByUserId(string userId);
        Task<Follow> AddFollower(Follow follow);
        Task RemoveFollower(string followerId);
    }
}
