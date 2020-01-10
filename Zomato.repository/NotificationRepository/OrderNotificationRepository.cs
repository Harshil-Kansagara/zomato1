using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Data;
using Zomato.DomainModel.Models;
using Zomato.Repository.DataRepository;

namespace Zomato.Repository.NotificationRepository
{
    public class OrderNotificationRepository : IOrderNotificationRepository
    {
        private IDataRepository _dataRepository;

        public OrderNotificationRepository(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public async Task AddConnectionId(NotificationHub notificationHub)
        {
            await _dataRepository.AddAsync(notificationHub);
        }

        public async Task AddOrderDataForNotification(OrderNotificationData orderNotificationData)
        {
            await _dataRepository.AddAsync(orderNotificationData);
        }

        public async Task<List<NotificationHub>> GetConnectionList()
        {
            return await _dataRepository.Get<NotificationHub>();
        }

        public async Task<List<OrderNotificationData>> GetOrderNotification()
        {
            var a = await _dataRepository.Get<OrderNotificationData>();
            if(a == null)
            {
                return a;
            }
            return a;
        }

        public async Task RemoveConnectionId(NotificationHub notificationHub)
        {
            var connectionList = await _dataRepository.Where<NotificationHub>(x => x.UserId == notificationHub.UserId && x.ConnectionId == notificationHub.ConnectionId).ToListAsync();
            if(connectionList != null) {
                foreach (var each in connectionList)
                {
                    _dataRepository.Remove(each);
                }
            }
        }

        public async Task RemoveOrderNotificationData(int orderId)
        {
            var orderList = await _dataRepository.Where<OrderNotification>(x => x.OrderId == orderId).ToListAsync();
            if (orderList != null)
            {
                foreach (var each in orderList)
                {
                    _dataRepository.Remove(each);
                }
            }
        }
    }
}
