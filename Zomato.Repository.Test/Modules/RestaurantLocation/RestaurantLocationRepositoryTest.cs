using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Zomato.Repository.DataRepository;
using Zomato.Repository.RestaurantLocationRepository;
using Zomato.Repository.Test.Bootstrap;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;
using System.Linq.Expressions;
using System.Linq;
using MockQueryable.Moq;

namespace Zomato.Repository.Test.Modules.RestaurantLocationTesting
{
    [Collection("Register Dependency")]
    public class RestaurantLocationRepositoryTest
    {
        private Mock<IDataRepository> _dataRepositoryMock { get; }
        private IRestLocationRepository _restLocationRepository { get; }

        public RestaurantLocationRepositoryTest(Initialize initialize)
        {
            _dataRepositoryMock = initialize.serviceProvider.GetService<Mock<IDataRepository>>();
            _restLocationRepository = initialize.serviceProvider.GetService<IRestLocationRepository>();
        }

        [Fact]
        public async Task AddRestLocation_Verify()
        {
            var restLocation = new RestaurantLocation();
            await _restLocationRepository.AddRestLocation(restLocation);
            _dataRepositoryMock.Verify(x => x.AddAsync(It.IsAny<RestaurantLocation>()), Times.Once);
        }

        [Fact]
        public async Task GetRestLocationById()
        {
            var givenRestaurantId = 2;
            var restLocationList = new List<RestaurantLocation> {
                new RestaurantLocation
                {
                    LocationId = 4,
                    RestaurantId = 2,
                    Location = "Vadodara"
                }
            };

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<RestaurantLocation, bool>>>())).Returns(restLocationList.AsQueryable().BuildMock().Object);

            var restaurants = await _restLocationRepository.GetRestLocationById(givenRestaurantId);
            Assert.NotEmpty(restaurants);
        }
    }
}
