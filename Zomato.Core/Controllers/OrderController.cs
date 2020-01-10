
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zomato.Core.Hubs;
using Zomato.DomainModel.Models;
using Zomato.Repository.UnitofWork;

namespace Zomato.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private IHubContext<OrderHub> _orderHub;

        public OrderController(IUnitOfWork unitOfWork, IHubContext<OrderHub> orderHub)
        {
            _unitOfWork = unitOfWork;
            _orderHub = orderHub;
        }

        [HttpPost]
        [Route("{restaurantName}/order")]
        public async Task<IActionResult> AddOrder(OrderedData newOrder, string restaurantName)
        {
            if (ModelState.IsValid) {
                DateTime today = DateTime.Today;
                Order order = new Order();
                order.RestaurantId = await _unitOfWork.RestaurantRepository.GetRestaurantIdByRestaurantName(restaurantName);
                order.OrderDate = today.ToString("dd-MM-yyyy");
                order.UserId = newOrder.UserId;
                order.AddressId = newOrder.AddressId;

                order = await _unitOfWork.OrderRepository.AddOrder(order);
                _unitOfWork.commit();
                for (int i = 0; i < newOrder.Items.Count; i++)
                {
                    OrderedItem orderedItem = new OrderedItem();
                    orderedItem.ItemId = newOrder.Items[i].ItemId;
                    orderedItem.ItemQuantity = newOrder.Items[i].ItemQuantity;
                    orderedItem.OrderId = order.OrderId;

                    await _unitOfWork.OrderedItemRepository.AddOrderedItem(orderedItem);

                    _unitOfWork.commit();
                }

                var connectionList = await _unitOfWork.OrderNotificationRepository.GetConnectionList();

                foreach (var each in connectionList)
                {
                    if(each.UserId == "4aa56cd4-3ac4-4be0-af99-5933372d8a22")
                    {
                        OrderNotification orderNotification = new OrderNotification();
                        orderNotification.OrderId = order.OrderId;
                        orderNotification.RestaurantName = restaurantName;
                        await _orderHub.Clients.Client(each.ConnectionId).SendAsync("OrderReceived", orderNotification);
                    }
                }
                OrderNotificationData orderNotificationData = new OrderNotificationData();
                orderNotificationData.OrderId = order.OrderId;
                await _unitOfWork.OrderNotificationRepository.AddOrderDataForNotification(orderNotificationData);
                _unitOfWork.commit();
               
                 return Ok(order);
                
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("{orderId}")]
        public async Task<OrderDetail> GetOrderDetail(int orderId)
        {
            string itemName = null;
            int itemPrice = 0;
            OrderDetail orderDetail = new OrderDetail();
            Order order = new Order();
            orderDetail.TotalAmount = 0;
            order = await _unitOfWork.OrderRepository.GetOrderDataByOrderId(orderId);
            IdentityUser user = await _unitOfWork.UserRepository.GetUserDetail(order.UserId);
            List<OrderedItem> orderedItem = await _unitOfWork.OrderedItemRepository.GetOrderedItemByOrderId(order.OrderId);

            for (int i = 0; i < orderedItem.Count; i++)
            {
                itemName = await _unitOfWork.MenuRepository.GetMenuNameByItemId(orderedItem[i].ItemId);
                itemPrice = await _unitOfWork.MenuRepository.GetItemPriceByItemId(orderedItem[i].ItemId);

                orderDetail.ItemDetail.Add(new ItemDetail(orderedItem[i].OrderId, itemName, orderedItem[i].ItemQuantity, itemPrice));
                orderDetail.TotalAmount = orderDetail.TotalAmount+(orderedItem[i].ItemQuantity * itemPrice);
            }

            orderDetail.OrderId = order.OrderId;
            orderDetail.Date = order.OrderDate;
            orderDetail.RestaurantName = await _unitOfWork.RestaurantRepository.GetRestaurantNameById(order.RestaurantId);
            orderDetail.UserName = user.UserName;
            orderDetail.UserNumber = user.PhoneNumber;
            orderDetail.UserEmail = user.Email;
            orderDetail.DeliveryLocation = await _unitOfWork.UserAddressRepository.GetAddressNameById(order.AddressId);

            return orderDetail;
        }

        [HttpGet]
        [Route("restaurant/{restaurantName}")]
        public async Task<List<OrderDetail>> GetOrderList(string restaurantName)
        {
            string itemName = null;
            int itemPrice = 0;
            List<OrderDetail> orderDetails = new List<OrderDetail>();

            var restaurantId = await _unitOfWork.RestaurantRepository.GetRestaurantIdByRestaurantName(restaurantName);

            var orderList = await _unitOfWork.OrderRepository.GetOrdersByRestaurantId(restaurantId);

            //IdentityUser user = await _unitOfWork.UserRepository.GetUserDetail(userId);
            foreach (var each in orderList)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.TotalAmount = 0;

                IdentityUser user = await _unitOfWork.UserRepository.GetUserDetail(each.UserId);

                List<OrderedItem> orderedItem = await _unitOfWork.OrderedItemRepository.GetOrderedItemByOrderId(each.OrderId);
                for (int i = 0; i < orderedItem.Count; i++)
                {
                    itemName = await _unitOfWork.MenuRepository.GetMenuNameByItemId(orderedItem[i].ItemId);
                    itemPrice = await _unitOfWork.MenuRepository.GetItemPriceByItemId(orderedItem[i].ItemId);

                    orderDetail.ItemDetail.Add(new ItemDetail(orderedItem[i].OrderId, itemName, orderedItem[i].ItemQuantity, itemPrice));

                    orderDetail.TotalAmount = orderDetail.TotalAmount + (orderedItem[i].ItemQuantity * itemPrice);
                }
                orderDetail.OrderId = each.OrderId;
                orderDetail.Date = each.OrderDate;
                orderDetail.RestaurantName = restaurantName;
                orderDetail.UserName = user.UserName;
                orderDetail.UserNumber = user.PhoneNumber;
                orderDetail.UserEmail = user.Email;
                orderDetail.DeliveryLocation = await _unitOfWork.UserAddressRepository.GetAddressNameById(each.AddressId);

                orderDetails.Add(orderDetail);
            }

            return orderDetails;
        }

        [HttpGet]
        [Route("user/{userId}")]
        public async Task<List<OrderDetail>> GetOrderListofUser(string userId)
        {
            string itemName = null;
            int itemPrice = 0;
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            IdentityUser user = await _unitOfWork.UserRepository.GetUserDetail(userId);
            var orderList = await _unitOfWork.OrderRepository.GetOrdersByUserId(userId);
            foreach (var each in orderList)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.TotalAmount = 0;
                List<OrderedItem> orderedItem = await _unitOfWork.OrderedItemRepository.GetOrderedItemByOrderId(each.OrderId);
                for (int i = 0; i < orderedItem.Count; i++)
                {
                    itemName = await _unitOfWork.MenuRepository.GetMenuNameByItemId(orderedItem[i].ItemId);
                    itemPrice = await _unitOfWork.MenuRepository.GetItemPriceByItemId(orderedItem[i].ItemId);

                    orderDetail.ItemDetail.Add(new ItemDetail(orderedItem[i].OrderId, itemName, orderedItem[i].ItemQuantity, itemPrice));

                    orderDetail.TotalAmount = orderDetail.TotalAmount + (orderedItem[i].ItemQuantity * itemPrice);
                }
                orderDetail.OrderId = each.OrderId;
                orderDetail.Date = each.OrderDate;
                orderDetail.RestaurantName = await _unitOfWork.RestaurantRepository.GetRestaurantNameById(each.RestaurantId);
                orderDetail.UserName = user.UserName;
                orderDetail.UserNumber = user.PhoneNumber;
                orderDetail.UserEmail = user.Email;
                orderDetail.DeliveryLocation = await _unitOfWork.UserAddressRepository.GetAddressNameById(each.AddressId);

                orderDetails.Add(orderDetail);
            }
            return orderDetails;
        }

        [HttpDelete]
        [Route("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            try { 
                await _unitOfWork.OrderRepository.DeleteOrder(orderId);
                await _unitOfWork.OrderedItemRepository.DeleteOrderItem(orderId);
                _unitOfWork.commit();
                return Ok();
            }catch(Exception ex)
            {
                throw ex;
            }
        } 
    }
}
