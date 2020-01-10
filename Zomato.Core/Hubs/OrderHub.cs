using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;
using Zomato.Repository.UnitofWork;

namespace Zomato.Core.Hubs
{
    public class OrderHub : Hub
    {
        //private readonly static ConnectionMapping<string> _connections =
        //new ConnectionMapping<string>();

        private IUnitOfWork _unitOfWork;

        public OrderHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AdminNotification(bool a)
        {
            if (a) {
                var orderNotificationList = await _unitOfWork.OrderNotificationRepository.GetOrderNotification();
                var connectionList = await _unitOfWork.OrderNotificationRepository.GetConnectionList();
                foreach (var each in connectionList)
                {
                    if (each.UserId == "4aa56cd4-3ac4-4be0-af99-5933372d8a22")
                    {
                        foreach (var each1 in orderNotificationList)
                        {
                            var restaurantName = await _unitOfWork.RestaurantRepository.GetRestaurantNameById(await _unitOfWork.OrderRepository.GetRestaurantIdByOrderId(each1.OrderId));
                            OrderNotification orderNotification = new OrderNotification();
                            orderNotification.OrderId = each1.OrderId;
                            orderNotification.RestaurantName = restaurantName;
                            await Clients.Client(each.ConnectionId).SendAsync("OrderReceived", orderNotification);
                        }
                        
                    }
                }
            }
        }

        public async Task DeliveryOrder(int orderId)
        {
            var userId = await _unitOfWork.OrderRepository.GetUserIdByOrderId(orderId);
            var connectionList = await _unitOfWork.OrderNotificationRepository.GetConnectionList();
            foreach (var each in connectionList)
            {
                if(each.UserId == userId)
                {
                    await _unitOfWork.OrderNotificationRepository.RemoveOrderNotificationData(orderId);
                    _unitOfWork.commit();
                    await Clients.Client(each.ConnectionId).SendAsync("DeliverySuccessful", "Delivered Order");
                }
            }
        }

        public override async Task OnConnectedAsync()
        {
            if(Context.User.Identity.Name != null)
            {
                NotificationHub notificationHub = new NotificationHub();
                notificationHub.UserId = Context.User.Identity.Name;
                notificationHub.ConnectionId = Context.ConnectionId;
                await _unitOfWork.OrderNotificationRepository.AddConnectionId(notificationHub);
                _unitOfWork.commit();
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if(Context.User.Identity.Name != null)
            {
                NotificationHub notificationHub = new NotificationHub();
                notificationHub.UserId = Context.User.Identity.Name;
                notificationHub.ConnectionId = Context.ConnectionId;
                await _unitOfWork.OrderNotificationRepository.RemoveConnectionId(notificationHub); 
                _unitOfWork.commit();
            }
        }

        
    }
}
