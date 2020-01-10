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
    public class RestaurantController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;

        public RestaurantController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("restaurant")]
        public async Task<List<RestaurantCollection>> Index()
        {

            List<RestaurantCollection> _restaurantCollection = new List<RestaurantCollection>();
            var restaurantList = await _unitOfWork.RestaurantRepository.ListRestaurant();

            foreach (var restaurant in restaurantList)
            {
                var totalRating = 0;
                var model = new RestaurantCollection();
                model.Restaurant = restaurant;

                var _restCategory = await _unitOfWork.RestCategoryRepository.GetRestCategoryByRestaurantId(restaurant.RestaurantId);

                for (int i = 0; i < _restCategory.Count; i++)
                {
                    model.Categories.Add(await _unitOfWork.CategoryRepository.GetCategoryById(_restCategory[i].CategoryId));
                }

                var _restCuisine = await _unitOfWork.RestCuisineRepository.GetRestCuisinesByRestaurantId(restaurant.RestaurantId);

                for (int i = 0; i < _restCuisine.Count; i++)
                {
                    model.Cuisines.Add(_unitOfWork.CuisineRepository.GetCuisineById(_restCuisine[i].CuisineId).Result.CuisineName);
                }

                var _restLocation = await _unitOfWork.RestLocationRepository.GetRestLocationById(restaurant.RestaurantId);

                for (int i = 0; i < _restLocation.Count; i++)
                {
                    model.RestaurantLocation.Add(_restLocation[i].Location);
                }

                var _restReviews = await _unitOfWork.ReviewRepository.GetReviewByRestaurantId(restaurant.RestaurantId);

                if (_restReviews.Count != 0) {
                    foreach (var review in _restReviews)
                    {
                        totalRating += review.rating;
                    }

                    model.RatingAvg = (double)totalRating / _restReviews.Count;
                }
                else
                {
                    model.RatingAvg = 0.00;
                }
                _restaurantCollection.Add(model);
            }

            return _restaurantCollection;
        }

        [HttpGet]
        [Route("list")]
        public async Task<List<Restaurant>> List()
        {
            return await _unitOfWork.RestaurantRepository.ListRestaurant();
        }

        [HttpGet]
        [Route("restaurant/{restaurantName}")]
        public async Task<IActionResult> CheckRestaurant(string restaurantName)
        {
            var restaurantId = await _unitOfWork.RestaurantRepository.GetRestaurantIdByRestaurantName(restaurantName);
            if(restaurantId == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet]
        [Route("{restaurantName}")]
        public async Task<RestaurantCollection> Detail(string restaurantName)
        {
            var totalRating = 0;

            var restaurantId = await _unitOfWork.RestaurantRepository.GetRestaurantIdByRestaurantName(restaurantName);

            RestaurantCollection _restaurantCollection = new RestaurantCollection();

            _restaurantCollection.Restaurant = await _unitOfWork.RestaurantRepository.GetRestaurantById(restaurantId);

            var _restCategory = await _unitOfWork.RestCategoryRepository.GetRestCategoryByRestaurantId(restaurantId);

            var _restCuisine = await _unitOfWork.RestCuisineRepository.GetRestCuisinesByRestaurantId(restaurantId);

            var _restLocation = await _unitOfWork.RestLocationRepository.GetRestLocationById(restaurantId);

            var _restReviews = await _unitOfWork.ReviewRepository.GetReviewByRestaurantId(restaurantId);

            for (int i = 0; i < _restCategory.Count; i++)
            {
                _restaurantCollection.Categories.Add(await _unitOfWork.CategoryRepository.GetCategoryById(_restCategory[i].CategoryId));
            }

            for (int i = 0; i < _restCuisine.Count; i++)
            {
                _restaurantCollection.Cuisines.Add(_unitOfWork.CuisineRepository.GetCuisineById(_restCuisine[i].CuisineId).Result.CuisineName);
            }

            for (int i = 0; i < _restLocation.Count; i++)
            {
                _restaurantCollection.RestaurantLocation.Add(_restLocation[i].Location);
            }

            if (_restReviews.Count != 0)
            {
                foreach (var review in _restReviews)
                {
                    totalRating += review.rating;
                }

                _restaurantCollection.RatingAvg = (double)totalRating / _restReviews.Count;
            }
            else
            {
                _restaurantCollection.RatingAvg = 0.00;
            }
            return _restaurantCollection;
        }

        [HttpPost]
        [Route("restaurant")]
        public async Task<IActionResult> Create(NewRestaurant newRestaurant)
        {
            var rest = await _unitOfWork.RestaurantRepository.ListRestaurant();
            Restaurant restaurant = new Restaurant();

            if (ModelState.IsValid) {

                foreach (var item in rest)
                {
                    if(item.RestaurantName == newRestaurant.RestaurantName)
                    {
                        return BadRequest();
                    }
                }

                try {
                    restaurant.RestaurantName = newRestaurant.RestaurantName;
                    restaurant = await _unitOfWork.RestaurantRepository.AddRestaurant(restaurant);

                    for (int i = 0; i < newRestaurant.Location.Count; i++) { 
                        RestaurantLocation restaurantLocation = new RestaurantLocation();
                        restaurantLocation.RestaurantId = restaurant.RestaurantId;
                        restaurantLocation.Location = newRestaurant.Location[i];
                        await _unitOfWork.RestLocationRepository.AddRestLocation(restaurantLocation);
                    }
            
                    for(int i = 0; i < newRestaurant.CategoryId.Count; i++) { 
                        RestCategory restCategory = new RestCategory();
                        restCategory.CategoryId = newRestaurant.CategoryId[i];
                        restCategory.RestaurantId = restaurant.RestaurantId;
                        await _unitOfWork.RestCategoryRepository.AddRestCategory(restCategory);
                    }

                    for(int i=0;i<newRestaurant.CuisineId.Count; i++) { 
                        RestCuisine restCuisine = new RestCuisine();
                        restCuisine.CuisineId = newRestaurant.CuisineId[i];
                        restCuisine.RestaurantId = restaurant.RestaurantId;
                        await _unitOfWork.RestCuisineRepository.AddRestCuisine(restCuisine);
                    }
                
                    _unitOfWork.commit();
                    return Ok();
                }
                catch(Exception e)
                {
                    throw e;
                }
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("{restaurantId}")]
        public async Task DeleteConfirm(int restaurantId)
        {
            try
            {
                await _unitOfWork.RestaurantRepository.deleteRestaurant(restaurantId);
                await _unitOfWork.MenuRepository.DeleteMenuByRestaurantId(restaurantId);
                await _unitOfWork.OrderRepository.DeleteOrderByRestaurant(restaurantId);
                var reviewList = await _unitOfWork.ReviewRepository.GetReviewByRestaurantId(restaurantId);
                foreach (var each in reviewList)
                {
                    var commentList = await _unitOfWork.CommentRepository.GetCommentByReviewId(each.ReviewId);
                    var likeList = await _unitOfWork.LikeRepository.GetLikeByReviewId(each.ReviewId);
                    await _unitOfWork.ReviewRepository.DeleteReview(each.ReviewId);
                    foreach (var comment in commentList)
                    {
                        await _unitOfWork.CommentRepository.DeleteComment(comment.CommentId);
                    }
                    foreach (var like in likeList)
                    {
                        await _unitOfWork.LikeRepository.DeleteLike(like.LikeId);
                    }
                }
                _unitOfWork.commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
