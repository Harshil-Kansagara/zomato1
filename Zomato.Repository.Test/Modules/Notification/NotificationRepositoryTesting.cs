using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Zomato.Repository.Test.Bootstrap;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Zomato.Repository.DataRepository;
using Zomato.Repository.NotificationRepository;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;
using System.Linq;
using MockQueryable.Moq;
using System.Linq.Expressions;

namespace Zomato.Repository.Test.Modules.NotificationTesting
{
    [Collection("Register Dependency")]
    public class NotificationRepositoryTesting
    {
        private Mock<IDataRepository> _dataRepositoryMock { get; }
        private IOrderNotificationRepository _notificationRepository { get; }

        public NotificationRepositoryTesting(Initialize initialize)
        {
            _dataRepositoryMock = initialize.serviceProvider.GetService<Mock<IDataRepository>>();
            _notificationRepository = initialize.serviceProvider.GetService<IOrderNotificationRepository>();
        }

        [Fact]
        public async Task AddConnectionId_Verify()
        {
            var notificationHub = new NotificationHub();
            await _notificationRepository.AddConnectionId(notificationHub);
            _dataRepositoryMock.Verify(x => x.AddAsync(It.IsAny<NotificationHub>()), Times.Once);
        }

        [Fact]
        public async Task AddOrderDataForNotification_Verify()
        {
            var orderNotificationData = new OrderNotificationData();
            await _notificationRepository.AddOrderDataForNotification(orderNotificationData);
            _dataRepositoryMock.Verify(x => x.AddAsync(It.IsAny<OrderNotificationData>()), Times.Once);
        }

        [Fact]
        public async Task GetConnectionList_IsNotNull()
        {
            var notificationHub = new List<NotificationHub> {
                new NotificationHub
                {
                    Id = 1
                }
            };
            _dataRepositoryMock.Setup(x => x.Get<NotificationHub>()).Returns(Task.FromResult(notificationHub));
            var notificationData = await _notificationRepository.GetConnectionList();
            Assert.NotNull(notificationData);
        }

        [Fact]
        public async Task GetOrderNotification_IsNotNull()
        {
            var orderNotification = new List<OrderNotificationData>
            {
                new OrderNotificationData
                {
                    OrderId = 52
                }
            };
            _dataRepositoryMock.Setup(x => x.Get<OrderNotificationData>()).Returns(Task.FromResult(orderNotification));

            var orderNotificationData = await _notificationRepository.GetOrderNotification();

            Assert.NotNull(orderNotificationData);
        }

        [Fact]
        public async Task RemoveConnectionId_Verify()
        {
            var notification = new NotificationHub();
            var notificationHub = new List<NotificationHub>();
            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<NotificationHub, bool>>>())).Returns(notificationHub.AsQueryable().BuildMock().Object);
            await _notificationRepository.RemoveConnectionId(notification);
            _dataRepositoryMock.Verify(x=>x.Remove(notification), Times.Exactly(notificationHub.Count));
        }

        [Fact]
        public async Task RemoveOrderNotificationData_Verify()
        {
            var givenOrderId = 1;
            var orderNotification = new List<OrderNotification>();
            _dataRepositoryMock.Setup(x=>x.Where(It.IsAny<Expression<Func<OrderNotification, bool>>>())).Returns(orderNotification.AsQueryable().BuildMock().Object);
            await _notificationRepository.RemoveOrderNotificationData(givenOrderId);
            _dataRepositoryMock.Verify(x => x.Remove(orderNotification), Times.Exactly(orderNotification.Count));
        }
    }
}
