using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Zomato.Repository.DataRepository;
using Zomato.Repository.RestaurantRepository;
using Zomato.Repository.Test.Bootstrap;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;
using System.Linq.Expressions;
using System.Linq;
using MockQueryable.Moq;

namespace Zomato.Repository.Test.Modules.RestaurantTesting
{
    [Collection("Register Dependency")]
    public class RestaurantRepositoryTest
    {
        private Mock<IDataRepository> _dataRepositoryMock { get; }
        private IRestaurantRepository _restaurantRepository { get; }

        public RestaurantRepositoryTest(Initialize initialize)
        {
            _dataRepositoryMock = initialize.serviceProvider.GetService<Mock<IDataRepository>>();
            _restaurantRepository = initialize.serviceProvider.GetService<IRestaurantRepository>();
        }

        [Fact]
        public async Task AddRestaurant_Verify()
        {
            var restaurant = new Restaurant();
            await _restaurantRepository.AddRestaurant(restaurant);
            _dataRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Restaurant>()), Times.Once);
        }

        [Fact]
        public async Task DeleteRestaurant_Verify()
        {
            var givenRestaurantId = 2;
            var restaurant = new Restaurant();
            _dataRepositoryMock.Setup(x => x.Find<Restaurant>(givenRestaurantId)).Returns(Task.FromResult(restaurant));
            await _restaurantRepository.deleteRestaurant(givenRestaurantId);
            _dataRepositoryMock.Verify(x => x.Remove(restaurant), Times.Once);
        }

        [Fact]
        public async Task GetRestaurantById_IsNotNull()
        {
            var givenRestaurantId = 2;
            var expectedRestaurant = new Restaurant();
            expectedRestaurant.RestaurantId = 2;
            expectedRestaurant.RestaurantName = "Pizza Hut"; 

            _dataRepositoryMock.Setup(x => x.Find<Restaurant>(givenRestaurantId)).Returns(Task.FromResult(expectedRestaurant));
            var restaurant = await _restaurantRepository.GetRestaurantById(givenRestaurantId);

            Assert.NotNull(restaurant);
        }

        [Fact]
        public async Task GetRestaurantNameById_IsEqual()
        {
            var givenRestaurantId = 2;
            var expectedRestaurantName = "Pizza Hut";

            var expectedRestaurant = new Restaurant();
            expectedRestaurant.RestaurantId = 2;
            expectedRestaurant.RestaurantName = "Pizza Hut";

            _dataRepositoryMock.Setup(x => x.Find<Restaurant>(givenRestaurantId)).Returns(Task.FromResult(expectedRestaurant));

            var restaurantName = await _restaurantRepository.GetRestaurantNameById(givenRestaurantId);

            Assert.Equal(restaurantName, expectedRestaurantName);
        }

        [Fact]
        public async Task ListRestaurant_IsNotNull()
        {
            var listRestaurant = new List<Restaurant> {
                new Restaurant
                {
                    RestaurantId = 2,
                    RestaurantName = "Pizza Hut"
                }
            };
            _dataRepositoryMock.Setup(x => x.Get<Restaurant>()).Returns(Task.FromResult(listRestaurant));
            var restaurantList = await _restaurantRepository.ListRestaurant();

            Assert.NotNull(restaurantList);
        }

        [Fact]
        public async Task GetRestaurantIdByRestaurantName_IsEqual()
        {
            var givenRestaurantName = "Pizza Hut";
            var expectedRestaurantId = 2;

            var listRestaurant = new List<Restaurant> {
                new Restaurant
                {
                    RestaurantId = 2,
                    RestaurantName = "Pizza Hut"
                }
            };
            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Restaurant, bool>>>())).Returns(listRestaurant.AsQueryable().BuildMock().Object);

            var restaurantId = await _restaurantRepository.GetRestaurantIdByRestaurantName(givenRestaurantName);

            Assert.Equal(restaurantId, expectedRestaurantId);
        }
    }
}
