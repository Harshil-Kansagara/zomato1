using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Data;
using Zomato.DomainModel.Models;
using Zomato.Repository.DataRepository;

namespace Zomato.Repository.RestaurantLocationRepository
{
    public class RestLocationRepository : IRestLocationRepository
    {
        private IDataRepository _dataRepository;

        public RestLocationRepository(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public async Task<RestaurantLocation> AddRestLocation(RestaurantLocation restaurantLocation)
        {
            await _dataRepository.AddAsync(restaurantLocation);
            return restaurantLocation;
        }

        public async Task<List<RestaurantLocation>> GetRestLocationById(int restaurantId)
        {
            var a = await _dataRepository.Where<RestaurantLocation>(x => x.RestaurantId == restaurantId).ToListAsync();
            if(a == null)
            {
                return null;
            }
            return a;
        }
    }
}
