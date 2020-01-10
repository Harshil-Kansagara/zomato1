using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Data;
using Zomato.DomainModel.Models;
using Zomato.Repository.DataRepository;

namespace Zomato.Repository.MenuRepository
{
    public class MenuRepository : IMenuRepository
    {
        private IDataRepository _dataRepository;

        public MenuRepository(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public async Task<Menu> AddMenuItem(Menu menu)
        {
            await _dataRepository.AddAsync(menu);
            return menu;
        }

        public async Task DeleteMenu(int itemId)
        {
            var menu = await _dataRepository.Find<Menu>(itemId);
            if (menu != null)
            {
                _dataRepository.Remove(menu);
            }
        }

        public async Task DeleteMenuByRestaurantId(int restaurantId)
        {
            var menu = await _dataRepository.Where<Menu>(x=>x.RestaurantId==restaurantId).ToListAsync();
            if (menu != null)
            {
                foreach (var each in menu)
                {
                    _dataRepository.Remove(each);
                }
            }
        }

        public async Task<int> GetItemPriceByItemId(int itemId)
        {
            var menu = await _dataRepository.Where<Menu>(x => x.ItemId == itemId).FirstOrDefaultAsync();
            return menu.ItemPrice;
        }

        public async Task<List<Menu>> GetMenuByRestIdAndCuisineId(int restaurantId, int cuisineId)
        {
            return await _dataRepository.Where<Menu>(x => x.RestaurantId == restaurantId && x.CuisineId == cuisineId).ToListAsync();
        }

        public async Task<string> GetMenuNameByItemId(int itemId)
        {
            var menu = await _dataRepository.Where<Menu>(x => x.ItemId == itemId).FirstOrDefaultAsync();
            return menu.ItemName;
        }
    }
}
