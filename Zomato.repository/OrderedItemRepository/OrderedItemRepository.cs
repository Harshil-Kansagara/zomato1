using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Data;
using Zomato.DomainModel.Models;
using Zomato.Repository.DataRepository;

namespace Zomato.Repository.OrderedItemRepository
{
    public class OrderedItemRepository : IOrderedItemRepository
    {
        private IDataRepository _dataRepository;

        public OrderedItemRepository(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public async Task<OrderedItem> AddOrderedItem(OrderedItem orderedItem)
        {
            await _dataRepository.AddAsync(orderedItem);
            return orderedItem;
        }

        public async Task DeleteOrderItem(int orderId)
        {
            var orderItem = await _dataRepository.Where<OrderedItem>(x => x.OrderId == orderId).ToListAsync();
            foreach(var each in orderItem)
            {
                _dataRepository.Remove(each);
            }
        }

        public async Task<List<OrderedItem>> GetOrderedItemByOrderId(int orderId)
        {
            var a =  await _dataRepository.Where<OrderedItem>(x => x.OrderId == orderId).ToListAsync();
            if(a.Count == 0)
            {
                return null;
            }
            return a;
            
        }
    }
}
