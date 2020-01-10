using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;

namespace Zomato.Repository.OrderRepository
{
    public interface IOrderRepository
    {
        Task<Order> AddOrder(Order order);
        Task<Order> GetOrderDataByOrderId(int orderId);
        Task<List<Order>> GetOrdersByRestaurantId(int restaurantId);
        Task DeleteOrder(int orderId);
        Task<List<Order>> GetOrdersByUserId(string userId);
        Task DeleteOrderByRestaurant(int restaurantId);
        Task<int> GetRestaurantIdByOrderId(int orderId);
        Task<string> GetUserIdByOrderId(int orderId);
    }
}
