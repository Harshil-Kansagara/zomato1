using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Data;
using Zomato.DomainModel.Models;
using Zomato.Repository.DataRepository;

namespace Zomato.Repository.RestCategoryRepository
{
    public class RestCategoryRepository : IRestCategoryRepository
    {
        private IDataRepository _dataRepository;

        public RestCategoryRepository( IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public async Task<RestCategory> AddRestCategory(RestCategory restCategory)
        {
            await _dataRepository.AddAsync(restCategory);
            return restCategory;
        }

        public async Task<List<RestCategory>> GetRestaurantIdByCategoryId(int categoryId)
        {
            var a = await _dataRepository.Where<RestCategory>(x => x.CategoryId == categoryId).ToListAsync();
            if(a.Count == 0)
            {
                return null;
            }
            return a;
        }

        public async Task<List<RestCategory>> GetRestCategoryByRestaurantId(int restaurantId)
        {
            var a = await _dataRepository.Where<RestCategory>(x => x.RestaurantId == restaurantId).ToListAsync();
            if(a.Count == 0)
            {
                return null;
            }
            return a;
        }
    }
}
