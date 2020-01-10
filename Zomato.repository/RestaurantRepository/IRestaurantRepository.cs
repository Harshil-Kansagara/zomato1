using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;

namespace Zomato.Repository.RestaurantRepository
{
    public interface IRestaurantRepository
    {
        Task<List<Restaurant>> ListRestaurant();
        Task<Restaurant> AddRestaurant(Restaurant restaurant);
        Task<Restaurant> GetRestaurantById(int restaurantId);
        Task<string> GetRestaurantNameById(int restaurantId);
        Task deleteRestaurant(int restaurantId);
        Task<int> GetRestaurantIdByRestaurantName(string restaurantName);
    }
}
