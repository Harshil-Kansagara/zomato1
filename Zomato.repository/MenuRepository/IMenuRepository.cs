using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;

namespace Zomato.Repository.MenuRepository
{
    public interface IMenuRepository
    {
        Task<List<Menu>> GetMenuByRestIdAndCuisineId(int restaurantId, int cuisineId);
        Task<Menu> AddMenuItem(Menu menu);
        Task DeleteMenu(int itemId);
        Task<string> GetMenuNameByItemId(int itemId);
        Task<int> GetItemPriceByItemId(int itemId);
        Task DeleteMenuByRestaurantId(int restaurantId);
    }
}
