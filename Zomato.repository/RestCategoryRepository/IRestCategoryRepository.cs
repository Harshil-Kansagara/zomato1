using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;

namespace Zomato.Repository.RestCategoryRepository
{
    public interface IRestCategoryRepository
    {
        Task<List<RestCategory>> GetRestCategoryByRestaurantId(int restaurantId);
        Task<RestCategory> AddRestCategory(RestCategory restCategory);
        Task<List<RestCategory>> GetRestaurantIdByCategoryId(int categoryId);
    }
}
