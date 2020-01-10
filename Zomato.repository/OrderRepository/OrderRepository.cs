using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Data;
using Zomato.DomainModel.Models;
using Zomato.Repository.DataRepository;

namespace Zomato.Repository.OrderRepository
{
    public class OrderRepository : IOrderRepository
    {
        private IDataRepository _dataRepository;

        public OrderRepository(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public async Task<Order> AddOrder(Order order)
        {
            await _dataRepository.AddAsync(order);
            return order;
        }

        public async Task DeleteOrder(int orderId)
        {
            var order = await _dataRepository.Find<Order>(orderId);
            if (order != null)
            {
                _dataRepository.Remove(order);
            }
        }

        public async Task DeleteOrderByRestaurant(int restaurantId)
        {
            var orderList = await _dataRepository.Where<Order>(x=>x.RestaurantId == restaurantId).ToListAsync();
            if (orderList != null)
            {
                foreach (var order in orderList)
                {
                    _dataRepository.Remove(order);
                }
            }
        }

        public async Task<Order> GetOrderDataByOrderId(int orderId)
        {
            var a = await _dataRepository.Find<Order>(orderId);
            if(a == null)
            {
                return null;
            }
            return a;
        }

        public async Task<List<Order>> GetOrdersByRestaurantId(int restaurantId)
        {
            var a = await _dataRepository.Where<Order>(x => x.RestaurantId == restaurantId).OrderByDescending(x => x.OrderDate).ToListAsync();
            if(a.Count == 0)
            {
                return null;
            }
            return a;
        }

        public async Task<List<Order>> GetOrdersByUserId(string userId)
        {
            var a = await _dataRepository.Where<Order>(x => x.UserId == userId).OrderByDescending(x=>x.OrderDate).ToListAsync();
            if (a.Count == 0)
            {
                return null;
            }
            return a;
        }

        public async Task<int> GetRestaurantIdByOrderId(int orderId)
        {
            var a = await _dataRepository.Where<Order>(x => x.OrderId == orderId).FirstAsync();
            return a.RestaurantId;
        }

        public async Task<string> GetUserIdByOrderId(int orderId)
        {
            var a = await _dataRepository.Where<Order>(x => x.OrderId == orderId).FirstAsync();
            return a.UserId;
        }
    }
}
