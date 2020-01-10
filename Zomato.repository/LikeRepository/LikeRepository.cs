using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Data;
using Zomato.DomainModel.Models;
using Zomato.Repository.DataRepository;

namespace Zomato.Repository.LikeRepository
{
    public class LikeRepository : ILikeRepository
    {
        private IDataRepository _dataRepository;

        public LikeRepository(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public async Task<Like> AddLike(Like like)
        {
            await _dataRepository.AddAsync(like) ;
            return like;
        }

        public async Task DeleteLike(int likeId)
        {
            var like = await _dataRepository.Find<Like>(likeId);
            if(like != null)
            {
                _dataRepository.Remove(like);
            }
        }

        public async Task DeleteLikeByReview(int reviewId)
        {
            var like = await _dataRepository.Where<Like>(x => x.ReviewId == reviewId).ToListAsync();
            foreach (var each in like)
            {
                _dataRepository.Remove(each);
            }
        }

        public async Task<List<Like>> GetLikeByReviewId(int reviewId)
        {
           var a = await _dataRepository.Where<Like>(x => x.ReviewId == reviewId).ToListAsync();
            return a;
        }
    }
}
