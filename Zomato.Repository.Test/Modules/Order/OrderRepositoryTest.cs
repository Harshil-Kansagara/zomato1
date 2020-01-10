using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Zomato.Repository.DataRepository;
using Zomato.Repository.OrderRepository;
using Zomato.Repository.Test.Bootstrap;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;
using System.Linq.Expressions;
using System.Linq;
using MockQueryable.Moq;

namespace Zomato.Repository.Test.Modules.OrderTesting
{
    [Collection("Register Dependency")]
    public class OrderRepositoryTest
    {
        private Mock<IDataRepository> _dataRepositoryMock { get; }
        private IOrderRepository _orderRepository { get; }

        public OrderRepositoryTest(Initialize initialize)
        {
            _dataRepositoryMock = initialize.serviceProvider.GetService<Mock<IDataRepository>>();
            _orderRepository = initialize.serviceProvider.GetService<IOrderRepository>();
        }

        [Fact]
        public async Task AddOrder_Verify()
        {
            var order = new Order();
            await _orderRepository.AddOrder(order);
            _dataRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public async Task DeleteOrder_Verify()
        {
            var givenOrderId = 1;
            var order = new Order();
            _dataRepositoryMock.Setup(x => x.Find<Order>(givenOrderId)).Returns(Task.FromResult(order));
            await _orderRepository.DeleteOrder(givenOrderId);
            _dataRepositoryMock.Verify(x => x.Remove(order), Times.Once);
        }

        [Fact]
        public async Task DeleteOrderByRestaurant_Verify()
        {
            var givenRestaurantId = 1;
            var orderList = new List<Order>();
            _dataRepositoryMock.Setup(x=>x.Where(It.IsAny<Expression<Func<Order, bool>>>())).Returns(orderList.AsQueryable().BuildMock().Object);

            await _orderRepository.DeleteOrderByRestaurant(givenRestaurantId);

            _dataRepositoryMock.Verify(x => x.Remove(orderList), Times.Exactly(orderList.Count));
        }

        [Fact]
        public async Task GetOrderDataByOrderId_IsNotNull()
        {
            var givenOrderId = 32;
            
            var expectedOrder = new Order();
            expectedOrder.OrderId = 32;
            expectedOrder.RestaurantId = 2;
            expectedOrder.UserId = "21d632cb-a9bd-4ead-bd9b-e25957e1c242";
            expectedOrder.AddressId = 1;
            expectedOrder.OrderDate = "12-11-2019";

            _dataRepositoryMock.Setup(x => x.Find<Order>(givenOrderId)).Returns(Task.FromResult(expectedOrder));
            var order = await _orderRepository.GetOrderDataByOrderId(givenOrderId);

            Assert.NotNull(order);
        }

        //True 
        [Fact]
        public async Task GetOrdersByRestaurantId_IsNotEmpty()
        {
            var givenRestaurantId = 1;
            var orderList = new List<Order> {
                new Order
                {
                    OrderId = 32,
                    RestaurantId = 2,
                    UserId = "21d632cb-a9bd-4ead-bd9b-e25957e1c242",
                    AddressId = 1,
                    OrderDate = "12-11-2019"
                }
            };

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Order, bool>>>())).Returns(orderList.AsQueryable().BuildMock().Object);

            var orders = await _orderRepository.GetOrdersByRestaurantId(givenRestaurantId);
            Assert.NotEmpty(orders);
        }

        [Fact]
        public async Task GetOrdersByUserId_IsNotEmpty()
        {
            var givenUserId = "21d632cb-a9bd-4ead-bd9b-e25957e1c242";
            var orderList = new List<Order> {
                new Order
                {
                    OrderId = 32,
                    RestaurantId = 2,
                    UserId = "21d632cb-a9bd-4ead-bd9b-e25957e1c242",
                    AddressId = 1,
                    OrderDate = "12-11-2019"
                }
            };

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Order, bool>>>())).Returns(orderList.AsQueryable().BuildMock().Object);

            var orders = await _orderRepository.GetOrdersByUserId(givenUserId);
            Assert.NotEmpty(orders);
        }

        [Fact]
        public async Task GetRestaurantIdByOrderId_IsEqual()
        {
            var givenOrderId = 32;
            var expectedRestaurantId = 2;
            var orderList = new List<Order> {
                new Order
                {
                    OrderId = 32,
                    RestaurantId = 2,
                    UserId = "21d632cb-a9bd-4ead-bd9b-e25957e1c242",
                    AddressId = 1,
                    OrderDate = "12-11-2019"
                }
            };

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Order, bool>>>())).Returns(orderList.AsQueryable().BuildMock().Object);

            int restaurantId = await _orderRepository.GetRestaurantIdByOrderId(givenOrderId);

            Assert.Equal(restaurantId, expectedRestaurantId);
        }

        [Fact]
        public async Task GetUserIdByOrderId_IsEqual()
        {
            var givenOrderId = 32;
            var expectedUserId = "21d632cb-a9bd-4ead-bd9b-e25957e1c242";
            var orderList = new List<Order> {
                new Order
                {
                    OrderId = 32,
                    RestaurantId = 2,
                    UserId = "21d632cb-a9bd-4ead-bd9b-e25957e1c242",
                    AddressId = 1,
                    OrderDate = "12-11-2019"
                }
            };

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Order, bool>>>())).Returns(orderList.AsQueryable().BuildMock().Object);

            string userId = await _orderRepository.GetUserIdByOrderId(givenOrderId);

            Assert.Equal(userId, expectedUserId);
        }
    }
}
