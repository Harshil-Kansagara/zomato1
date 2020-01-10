using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;

namespace Zomato.Repository.LikeRepository
{
    public interface ILikeRepository
    {
        Task<List<Like>> GetLikeByReviewId(int reviewId);
        Task<Like> AddLike(Like like);
        Task DeleteLike(int likeId);
        Task DeleteLikeByReview(int reviewId);
    }
}
