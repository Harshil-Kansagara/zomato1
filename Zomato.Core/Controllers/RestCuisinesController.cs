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
    public class RestCuisinesController
    {
        private IUnitOfWork _unitOfWork;

        public RestCuisinesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        //POST: /RestCuisines
        public async Task<List<RestaurantCollection>> RestaurantList(List<int> cuisinesId)
        {
            List<int> restaurantIds = new List<int>();
            List<RestaurantCollection> _restaurantCollection = new List<RestaurantCollection>();

            foreach (int id in cuisinesId)
            {
                var restCuisines = await _unitOfWork.RestCuisineRepository.GetRestaurantIdByCuisineId(id);
                foreach(var restCuisine in restCuisines)
                {
                    restaurantIds.Add(restCuisine.RestaurantId);
                }
            }

            foreach(int restaurantId in restaurantIds)
            {
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

                _restaurantCollection.Add(model);
            }

            return _restaurantCollection;
        }
    }
}
