using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Data;
using Zomato.DomainModel.Models;
using Zomato.Repository.DataRepository;

namespace Zomato.Repository.RestCuisineRepository
{
    public class RestCuisineRepository : IRestCuisineRepository
    {
        private IDataRepository _dataRepository;

        public RestCuisineRepository(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public async Task<RestCuisine> AddRestCuisine(RestCuisine restCuisine)
        {
            await _dataRepository.AddAsync(restCuisine);
            return restCuisine;
        }

        public async Task<List<RestCuisine>> GetRestaurantIdByCuisineId(int cuisineId)
        {
            var a = await _dataRepository.Where<RestCuisine>(x => x.CuisineId == cuisineId).ToListAsync();
            if(a.Count == 0)
            {
                return null;
            }
            return a;
        }

        public async Task<List<RestCuisine>> GetRestCuisinesByRestaurantId(int restaurantId)
        {
            var a = await _dataRepository.Where<RestCuisine>(x => x.RestaurantId == restaurantId).ToListAsync();
            if(a.Count == 0)
            {
                return null;
            }
            return a;
        }
    }
}
