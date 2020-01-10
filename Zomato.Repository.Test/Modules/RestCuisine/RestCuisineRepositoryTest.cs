using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Zomato.Repository.DataRepository;
using Zomato.Repository.RestCuisineRepository;
using Zomato.Repository.Test.Bootstrap;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;
using System.Linq.Expressions;
using System.Linq;
using MockQueryable.Moq;

namespace Zomato.Repository.Test.Modules.RestCuisineTesting
{
    [Collection("Register Dependency")]
    public class RestCuisineRepositoryTest
    {
        private Mock<IDataRepository> _dataRepositoryMock { get; }
        private IRestCuisineRepository _restCuisineRepository { get; }

        public RestCuisineRepositoryTest(Initialize initialize)
        {
            _dataRepositoryMock = initialize.serviceProvider.GetService<Mock<IDataRepository>>();
            _restCuisineRepository = initialize.serviceProvider.GetService<IRestCuisineRepository>();
        }

        [Fact]
        public async Task AddRestCuisine_Verify()
        {
            var restCuisine = new RestCuisine();
            await _restCuisineRepository.AddRestCuisine(restCuisine);
            _dataRepositoryMock.Verify(x => x.AddAsync(It.IsAny<RestCuisine>()), Times.Once);
        }

        [Fact]
        public async Task GetRestaurantIdByCuisineId_IsNotEmpty()
        {
            var givenCuisineId = 6;
            var restCuisineList = new List<RestCuisine> {
                new RestCuisine
                {
                    Id = 5,
                    RestaurantId = 2,
                    CuisineId =6
                }
            };

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<RestCuisine, bool>>>())).Returns(restCuisineList.AsQueryable().BuildMock().Object);

            var restCuisines = await _restCuisineRepository.GetRestaurantIdByCuisineId(givenCuisineId);
            Assert.NotEmpty(restCuisines);
        }

        [Fact]
        public async Task GetRestCuisineByRestaurantId_IsNotEmpty()
        {
            var givenRestaurantId = 2;
            var restCuisineList = new List<RestCuisine> {
                new RestCuisine
                {
                    Id = 5,
                    RestaurantId = 2,
                    CuisineId =6
                }
            };

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<RestCuisine, bool>>>())).Returns(restCuisineList.AsQueryable().BuildMock().Object);

            var restCuisines = await _restCuisineRepository.GetRestCuisinesByRestaurantId(givenRestaurantId);
            Assert.NotEmpty(restCuisines);
        }
    }
}
