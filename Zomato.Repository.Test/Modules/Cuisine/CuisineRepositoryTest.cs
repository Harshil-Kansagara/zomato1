using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Zomato.Repository.CuisineRepository;
using Zomato.Repository.DataRepository;
using Zomato.Repository.Test.Bootstrap;
using Zomato.DomainModel.Models;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using MockQueryable.Moq;

namespace Zomato.Repository.Test.Modules.CuisineTesting
{
    [Collection("Register Dependency")]
    public class CuisineRepositoryTest
    {
        private Mock<IDataRepository> _dataRepositoryMock { get; }
        private ICuisineRepository _cuisineRepository { get; }

        public CuisineRepositoryTest(Initialize initialize)
        {
            _dataRepositoryMock = initialize.serviceProvider.GetService<Mock<IDataRepository>>();
            _cuisineRepository = initialize.serviceProvider.GetService<ICuisineRepository>();
        }

        [Fact]
        public async Task GetCuisineList_IsNotNull()
        {
            var cuisineList = new List<Cuisine> {
                new Cuisine
                {
                    CuisineId =1,
                    CuisineName = "Chinese"
                }
            };
            _dataRepositoryMock.Setup(x => x.Get<Cuisine>()).Returns(Task.FromResult(cuisineList));

            cuisineList = await _cuisineRepository.CuisineList();

            Assert.NotNull(cuisineList);
        }

        [Fact]
        public async Task GetCuisineById_IsEqual()
        {
            var expectedCuisineName = "Chinese";
            var givenCuisineId = 1;
            var cuisines = new List<Cuisine>(){
                new Cuisine()
                {
                    CuisineId = 1,
                    CuisineName = "Chinese"
                }
               };

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Cuisine, bool>>>())).Returns(cuisines.AsQueryable().BuildMock().Object);

            var cuisineNameData = await _cuisineRepository.GetCuisineById(givenCuisineId);

            Assert.Equal(cuisineNameData.CuisineName, expectedCuisineName);
        }
    }
}
