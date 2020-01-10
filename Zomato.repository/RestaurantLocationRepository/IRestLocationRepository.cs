using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;

namespace Zomato.Repository.RestaurantLocationRepository
{
    public interface IRestLocationRepository
    {
        Task<List<RestaurantLocation>> GetRestLocationById(int restaurantId);
        Task<RestaurantLocation> AddRestLocation(RestaurantLocation restaurantLocation);
    }
}
