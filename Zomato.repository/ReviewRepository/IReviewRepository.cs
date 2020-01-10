using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;

namespace Zomato.Repository.ReviewRepository
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetReviewByRestaurantId(int restaurantId);
        Task<List<Review>> GetReviewByUserId(string userId);
        Task<Review> AddReview(Review newReview);
        Task DeleteReview(int reviewId);
    }
}
