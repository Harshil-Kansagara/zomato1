using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Data;
using Zomato.DomainModel.Models;
using Zomato.Repository.DataRepository;

namespace Zomato.Repository.CuisineRepository
{
    public class CuisineRepository : ICuisineRepository
    {
        private IDataRepository _dataRepository;

        public CuisineRepository(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public async Task<List<Cuisine>> CuisineList()
        {
            return await _dataRepository.Get<Cuisine>();
        }

        public async Task<Cuisine> GetCuisineById(int cuisineId)
        {
            return await _dataRepository.Where<Cuisine>(x => x.CuisineId == cuisineId).FirstAsync();
            //return cuisine.CuisineName;
        }
    }
}
