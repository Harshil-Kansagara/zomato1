using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Data;
using Zomato.DomainModel.Models;
using Zomato.Repository.DataRepository;

namespace Zomato.Repository.RestaurantRepository
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private IDataRepository _dataRepository;

        public RestaurantRepository(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public async Task<Restaurant> AddRestaurant(Restaurant restaurant)
        {
            await _dataRepository.AddAsync(restaurant);
            return restaurant;
        }

        public async Task deleteRestaurant(int restaurantId)
        {
            var restaurant = await _dataRepository.Find<Restaurant>(restaurantId);
            if(restaurant != null)
            {
                _dataRepository.Remove(restaurant);
            }
        }

        public async Task<Restaurant> GetRestaurantById(int restaurantId)
        {
            var a = await _dataRepository.Find<Restaurant>(restaurantId);
            if(a == null)
            {
                return null;
            }
            return a;
        }

        public async Task<string> GetRestaurantNameById(int restaurantId)
        {
            var restaurant = await _dataRepository.Find<Restaurant>(restaurantId);
            if(restaurant == null)
            {
                return null;
            }
            return restaurant.RestaurantName;
        }

        public async Task<List<Restaurant>> ListRestaurant()
        {
            return await _dataRepository.Get<Restaurant>();
        }

        public async Task<int> GetRestaurantIdByRestaurantName(string restaurantName)
        {
            var restaurant= await _dataRepository.Where<Restaurant>(x => x.RestaurantName == restaurantName).FirstOrDefaultAsync();
            if (restaurant != null) { 
                return restaurant.RestaurantId;
            }
            return 0;
        }
    }
}
