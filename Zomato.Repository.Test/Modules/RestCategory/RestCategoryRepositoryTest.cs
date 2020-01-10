using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Zomato.Repository.DataRepository;
using Zomato.Repository.RestCategoryRepository;
using Zomato.Repository.Test.Bootstrap;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;
using System.Linq.Expressions;
using System.Linq;
using MockQueryable.Moq;

namespace Zomato.Repository.Test.Modules.RestCategoryTesting
{ 
    [Collection("Register Dependency")]
    public class RestCategoryRepositoryTest
    {
        private Mock<IDataRepository> _dataRepositoryMock { get; }
        private IRestCategoryRepository _restCategoryRepository { get; }

        public RestCategoryRepositoryTest(Initialize initialize)
        {
            _dataRepositoryMock = initialize.serviceProvider.GetService<Mock<IDataRepository>>();
            _restCategoryRepository = initialize.serviceProvider.GetService<IRestCategoryRepository>();
        }

        [Fact]
        public async Task AddRestCategory_Verify()
        {
            var restCategory = new RestCategory();
            await _restCategoryRepository.AddRestCategory(restCategory);
            _dataRepositoryMock.Verify(x => x.AddAsync(It.IsAny<RestCategory>()), Times.Once);
        }

        [Fact]
        public async Task GetRestaurantIdByCategoryId_IsNotEmpty()
        {
            var givenCategoryId = 2;
            var restCategoryList = new List<RestCategory> {
                new RestCategory
                {
                    Id = 3,
                    RestaurantId = 2,
                    CategoryId =2
                }
            };

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<RestCategory, bool>>>())).Returns(restCategoryList.AsQueryable().BuildMock().Object);

            var restCategories = await _restCategoryRepository.GetRestaurantIdByCategoryId(givenCategoryId);
            Assert.NotEmpty(restCategories);
        }

        [Fact]
        public async Task GetRestCategoryByRestaurantId_IsNotEmpty()
        {
            var givenRestaurantId = 2;
            var restCategoryList = new List<RestCategory> {
                new RestCategory
                {
                    Id = 3,
                    RestaurantId = 2,
                    CategoryId =2
                }
            };

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<RestCategory, bool>>>())).Returns(restCategoryList.AsQueryable().BuildMock().Object);

            var restCategories = await _restCategoryRepository.GetRestCategoryByRestaurantId(givenRestaurantId);
            Assert.NotEmpty(restCategories);
        }
    }
}
