using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;

namespace Zomato.Repository.OrderedItemRepository
{
    public interface IOrderedItemRepository
    {
        Task<OrderedItem> AddOrderedItem(OrderedItem orderedItem);
        Task<List<OrderedItem>> GetOrderedItemByOrderId(int orderId);
        Task DeleteOrderItem(int orderId);
    }
}
