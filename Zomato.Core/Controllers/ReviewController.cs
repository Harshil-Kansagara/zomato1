using Microsoft.AspNetCore.Authorization;
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
    [AllowAnonymous]
    public class ReviewController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;

        public ReviewController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{restaurantName}/review")]
        public async Task<List<ReviewCollection>> GetReview (string restaurantName)
        {
            //restaurantName = restaurantName.Replace('-', ' ');
            var restaurantId = await _unitOfWork.RestaurantRepository.GetRestaurantIdByRestaurantName(restaurantName);

            List<ReviewCollection> reviewCollection = new List<ReviewCollection>();
            var reviewList = await _unitOfWork.ReviewRepository.GetReviewByRestaurantId(restaurantId);
           
            foreach(var review in reviewList)
            {
                var model = new ReviewCollection();
                model.Review = review;

                model.UserName =  _unitOfWork.UserRepository.GetUsernameByUserId(review.UserId).Result.UserName;

                model.LikeCount = _unitOfWork.LikeRepository.GetLikeByReviewId(review.ReviewId).Result.Count;

                var comments = await _unitOfWork.CommentRepository.GetCommentByReviewId(review.ReviewId);

                foreach (var comment in comments)
                {
                    var commentCollection = new CommentCollection();
                    commentCollection.CommentId = comment.CommentId;
                    commentCollection.CommentData = comment.CommentData;
                    commentCollection.UserName = _unitOfWork.UserRepository.GetUsernameByUserId(comment.UserId).Result.UserName;

                    model.Comments.Add(commentCollection);
                }

                model.CommentCount = comments.Count;

                reviewCollection.Add(model);
            }

            return reviewCollection;
        }

        [HttpGet]
        [Route("user/{userId}")]
        public async Task<List<ReviewCollection>> GetReviewofUser(string userId)
        {
            List<ReviewCollection> userReviewCollection = new List<ReviewCollection>();

            var reviewList = await _unitOfWork.ReviewRepository.GetReviewByUserId(userId);

            foreach (var review in reviewList)
            {
                var model = new ReviewCollection();
                model.Review = review;

                model.RestaurantName = await _unitOfWork.RestaurantRepository.GetRestaurantNameById(review.RestaurantId);

                model.LikeCount = _unitOfWork.LikeRepository.GetLikeByReviewId(review.ReviewId).Result.Count;

                var comments = await _unitOfWork.CommentRepository.GetCommentByReviewId(review.ReviewId);

                foreach (var comment in comments)
                {
                    var commentCollection = new CommentCollection();
                    commentCollection.CommentId = comment.CommentId;
                    commentCollection.CommentData = comment.CommentData;
                    commentCollection.UserName = _unitOfWork.UserRepository.GetUsernameByUserId(comment.UserId).Result.UserName;

                    model.Comments.Add(commentCollection);
                }


                model.CommentCount = model.Comments.Count;

                userReviewCollection.Add(model);
            }

            return userReviewCollection;
        }

        [HttpPost]
        [Route("{restaurantName}/review")]
        public async Task<IActionResult> AddReview(string restaurantName, Review newReview)
        {
            if (ModelState.IsValid) { 
                //restaurantName = restaurantName.Replace('-', ' ');
                var restaurantId = await _unitOfWork.RestaurantRepository.GetRestaurantIdByRestaurantName(restaurantName);
                newReview.RestaurantId = restaurantId;
                Review review = await _unitOfWork.ReviewRepository.AddReview(newReview);
                _unitOfWork.commit();
                return Ok(review);
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("{reviewId}")]
        public async Task DeleteReview(int reviewId)
        {
            try
            {
                await _unitOfWork.LikeRepository.DeleteLikeByReview(reviewId);
                await _unitOfWork.CommentRepository.DeleteComment(reviewId);
                await _unitOfWork.ReviewRepository.DeleteReview(reviewId);
                _unitOfWork.commit();
            }catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
