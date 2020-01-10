using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;
using Zomato.Repository.UnitofWork;

namespace Zomato.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;

        public FollowController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{userId}/follower")]
        public async Task<List<FollowCollection>> GetFollowers(string userId)
        {
            List<FollowCollection> followCollection = new List<FollowCollection>();
            var followers = await _unitOfWork.FollowRepository.GetFollowerByUserId(userId);

            foreach (var each in followers)
            {
                var model = new FollowCollection();
                model.UserId = each.UserId;
                model.UserName = _unitOfWork.UserRepository.GetUsernameByUserId(each.UserId).Result.UserName;
                followCollection.Add(model);
            }

            return followCollection;
        }

        [HttpGet]
        [Route("{userId}/following")]
        public async Task<List<FollowCollection>> GetFollowing(string userId)
        {
            List<FollowCollection> followCollection = new List<FollowCollection>();
            var following = await _unitOfWork.FollowRepository.GetFollowingByUserId(userId);
            foreach (var each in following)
            { 
                var model = new FollowCollection();
                model.UserId = each.FollowingId; 
                model.UserName = _unitOfWork.UserRepository.GetUsernameByUserId(each.FollowingId).Result.UserName;
                followCollection.Add(model);
            }

            return followCollection;
        }

        [HttpPost]
        public async Task<IActionResult> AddFollowers(Follow newFollow)
        {
            if (ModelState.IsValid) {
                var followList = await _unitOfWork.FollowRepository.GetFollowList();
                foreach (var each in followList)
                {
                    if(each.UserId == newFollow.UserId && each.FollowingId == newFollow.FollowingId)
                    {
                        return BadRequest();
                    }
                }

                Follow follow = await _unitOfWork.FollowRepository.AddFollower(newFollow);
                _unitOfWork.commit();
                return Ok(follow);
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("{followerId}")]
        public async Task DeleteFollower(string followerId)
        {
            try
            {
                await _unitOfWork.FollowRepository.RemoveFollower(followerId);
                _unitOfWork.commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
