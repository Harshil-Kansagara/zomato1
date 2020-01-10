using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Zomato.Repository.DataRepository;
using Zomato.Repository.OrderedItemRepository;
using Zomato.Repository.Test.Bootstrap;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;
using System.Linq.Expressions;
using System.Linq;
using MockQueryable.Moq;

namespace Zomato.Repository.Test.Modules.OrderedItemTesting
{
    [Collection("Register Dependency")]
    public class OrderedItemRepositoryTest
    {
        private Mock<IDataRepository> _dataRepositoryMock { get; }
        private IOrderedItemRepository _orderedItemRepository { get; }

        public OrderedItemRepositoryTest(Initialize initialize)
        {
            _dataRepositoryMock = initialize.serviceProvider.GetService<Mock<IDataRepository>>();
            _orderedItemRepository = initialize.serviceProvider.GetService<IOrderedItemRepository>();
        }

        [Fact]
        public async Task AddOrderedItem_Verify()
        {
            var orderItem = new OrderedItem();
            await _orderedItemRepository.AddOrderedItem(orderItem);
            _dataRepositoryMock.Verify(x => x.AddAsync(It.IsAny<OrderedItem>()), Times.Once);
        }

        [Fact]
        public async Task DeleteOrderItem_Verify()
        {
            var givenOrderId = 32;
            var orderItemList = new List<OrderedItem>();
            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<OrderedItem, bool>>>())).Returns(orderItemList.AsQueryable().BuildMock().Object);
            await _orderedItemRepository.DeleteOrderItem(givenOrderId);
            _dataRepositoryMock.Verify(x => x.Remove(orderItemList), Times.Exactly(orderItemList.Count));
        }

        [Fact]
        public async Task GetOrderedItemByOrderId_IsNotEmpty()
        {
            var givenOrderId = 32;
            var orderItemList = new List<OrderedItem>
            {
                new OrderedItem
                {
                    OrderItemId = 51,
                    OrderId = 32,
                    ItemId = 7,
                    ItemQuantity = 1
                },
                new OrderedItem
                {
                    OrderItemId = 52,
                    OrderId = 32,
                    ItemId = 2,
                    ItemQuantity = 1
                }
            };

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<OrderedItem, bool>>>())).Returns(orderItemList.AsQueryable().BuildMock().Object);

            var orderItems = await _orderedItemRepository.GetOrderedItemByOrderId(givenOrderId);
            Assert.NotEmpty(orderItems);
        }
    }
}
