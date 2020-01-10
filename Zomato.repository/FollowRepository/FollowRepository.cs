using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Data;
using Zomato.DomainModel.Models;
using Zomato.Repository.DataRepository;

namespace Zomato.Repository.FollowRepository
{
    public class FollowRepository : IFollowRepository
    {
        private IDataRepository _dataRepository;

        public FollowRepository(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public async Task<Follow> AddFollower(Follow follow)
        {
            await _dataRepository.AddAsync<Follow>(follow);
            return follow;
        }

        public async Task<List<Follow>> GetFollowingByUserId(string userId)
        {
            var a = await _dataRepository.Where<Follow>(x => x.UserId == userId).ToListAsync();
           
            return a;
        }

        public async Task<List<Follow>> GetFollowerByUserId(string userId)
        {
            
            var a = await _dataRepository.Where<Follow>(x => x.FollowingId == userId).ToListAsync();
            
            return a;
        }

        public async Task<List<Follow>> GetFollowList()
        {
            return await _dataRepository.Get<Follow>();
        }

        public async Task RemoveFollower(string followerId)
        {
            var followerList = await _dataRepository.Where<Follow>(x => x.FollowingId == followerId).ToListAsync();
            if (followerList != null)
            {
                foreach (var each in followerList)
                {
                    _dataRepository.Remove(each);
                }
            }
        }
    }
}
