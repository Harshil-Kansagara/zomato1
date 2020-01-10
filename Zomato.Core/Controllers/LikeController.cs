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
    public class LikeController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;

        public LikeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("review/{reviewId}/like")]
        public async Task<LikeCollection> GetLikeCollection(int reviewId)
        {
            string userName = null;
            LikeCollection likeCollection = new LikeCollection();
            likeCollection.Like = await _unitOfWork.LikeRepository.GetLikeByReviewId(reviewId);

            for(int i = 0; i < likeCollection.Like.Count; i++)
            {
                userName = _unitOfWork.UserRepository.GetUsernameByUserId(likeCollection.Like[i].UserId).Result.UserName;
                likeCollection.LikeDataCollection.Add(new LikeDataCollection(likeCollection.Like[i].LikeId, userName));
            }

            return likeCollection;
        }

        [HttpPost]
        [Route("like")]
        public async Task<IActionResult> AddLike (Like newLike)
        {
            if (ModelState.IsValid)
            {
                var likeList = await _unitOfWork.LikeRepository.GetLikeByReviewId(newLike.ReviewId);
                foreach (var like in likeList)
                {
                    if(like.UserId == newLike.UserId)
                    {
                        var likeId = like.LikeId;
                        await _unitOfWork.LikeRepository.DeleteLike(likeId);
                        _unitOfWork.commit();
                        return Ok();
                    }
                }

                var l = await _unitOfWork.LikeRepository.AddLike(newLike);
                _unitOfWork.commit();
                return Ok(l);
            }
            return BadRequest();
        }
    }
}
