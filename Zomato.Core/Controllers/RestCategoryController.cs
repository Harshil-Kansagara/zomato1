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
    public class RestCategoryController
    {
        private IUnitOfWork _unitOfWork;

        public RestCategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{categoryName}")]
        public async Task<List<RestaurantCollection>> GetRestaurantList(string categoryName)
        {
            var categoryId = await _unitOfWork.CategoryRepository.GetCategoryIdByName(categoryName);

            List<int> restaurantIds = new List<int>();
            List<RestaurantCollection> _restaurantCollection = new List<RestaurantCollection>();

            var restCategories = await _unitOfWork.RestCategoryRepository.GetRestaurantIdByCategoryId(categoryId);

            foreach (var restCategory in restCategories)
            {
                restaurantIds.Add(restCategory.RestaurantId);
            }

            foreach (int restaurantId in restaurantIds)
            {
                var totalRating = 0;
                var model = new RestaurantCollection();
                model.Restaurant = await _unitOfWork.RestaurantRepository.GetRestaurantById(restaurantId);

                var _restCategory = await _unitOfWork.RestCategoryRepository.GetRestCategoryByRestaurantId(restaurantId);

                for (int i = 0; i < _restCategory.Count; i++)
                {
                    model.Categories.Add(await _unitOfWork.CategoryRepository.GetCategoryById(_restCategory[i].CategoryId));
                }

                var _restCuisine = await _unitOfWork.RestCuisineRepository.GetRestCuisinesByRestaurantId(restaurantId);

                for (int i = 0; i < _restCuisine.Count; i++)
                {
                    model.Cuisines.Add(_unitOfWork.CuisineRepository.GetCuisineById(_restCuisine[i].CuisineId).Result.CuisineName);
                }

                var _restLocation = await _unitOfWork.RestLocationRepository.GetRestLocationById(restaurantId);

                for (int i = 0; i < _restLocation.Count; i++)
                {
                    model.RestaurantLocation.Add(_restLocation[i].Location);
                }

                var _restReviews = await _unitOfWork.ReviewRepository.GetReviewByRestaurantId(restaurantId);

                if (_restReviews.Count != 0)
                {
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
    }
}
