using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;

namespace Zomato.Repository.NotificationRepository
{
    public interface IOrderNotificationRepository
    {
        Task AddConnectionId(NotificationHub notificationHub);
        Task RemoveConnectionId(NotificationHub notificationHub);
        Task<List<NotificationHub>> GetConnectionList();
        Task AddOrderDataForNotification(OrderNotificationData orderNotificationData);
        Task<List<OrderNotificationData>> GetOrderNotification();
        Task RemoveOrderNotificationData(int orderId);
    }
}
