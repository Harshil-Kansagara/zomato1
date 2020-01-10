using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;

namespace Zomato.Repository.RestCuisineRepository
{
    public interface IRestCuisineRepository
    {
        Task<List<RestCuisine>> GetRestCuisinesByRestaurantId(int restaurantId);
        Task<RestCuisine> AddRestCuisine(RestCuisine restCuisine);
        Task<List<RestCuisine>> GetRestaurantIdByCuisineId(int cuisineId);
    }
}
