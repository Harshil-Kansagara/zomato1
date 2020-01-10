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
    public class CuisineController
    {
        private IUnitOfWork _unitOfWork;

        public CuisineController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<List<Cuisine>> CuisineList()
        {
            return await _unitOfWork.CuisineRepository.CuisineList();
        }

        [HttpGet]
        [Route("{restaurantName}")]
        public async Task<List<Cuisine>> CuisineListByRestaurant(string restaurantName)
        {
            List<Cuisine> cuisines = new List<Cuisine>();

            var restaurantId = await _unitOfWork.RestaurantRepository.GetRestaurantIdByRestaurantName(restaurantName);
            var resCuisineList = await _unitOfWork.RestCuisineRepository.GetRestCuisinesByRestaurantId(restaurantId);
            foreach (var each in resCuisineList)
            {
                var model = new Cuisine();
                model = await _unitOfWork.CuisineRepository.GetCuisineById(each.CuisineId);
                cuisines.Add(model);
            }

            return cuisines;
        }

        [HttpPost]
        public List<string> CuisineNameById(List<int> cuisineId)
        {
            List<string> cuisineName = new List<string>();
            foreach(int id in cuisineId)
            {
                cuisineName.Add(_unitOfWork.CuisineRepository.GetCuisineById(id).Result.CuisineName);
            }

            return cuisineName;
        }
    }
}
