using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Zomato.Repository.DataRepository;
using Zomato.Repository.MenuRepository;
using Zomato.Repository.Test.Bootstrap;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Zomato.DomainModel.Models;
using System.Linq;
using MockQueryable.Moq;
using Xunit;

namespace Zomato.Repository.Test.Modules.MenuTesting
{
    [Collection("Register Dependency")]
    public class MenuRepositoryTest
    {
        private Mock<IDataRepository> _dataRepositoryMock { get; }
        private IMenuRepository _menuRepository { get; }

        public MenuRepositoryTest(Initialize initialize)
        {
            _dataRepositoryMock = initialize.serviceProvider.GetService<Mock<IDataRepository>>();
            _menuRepository = initialize.serviceProvider.GetService<IMenuRepository>();
        }

        [Fact]
        public async Task GetItemPriceByItemId_Equal()
        {
            var givenItemId = 1;
            var expectedItemPrice = 200;
            var menu = new List<Menu> { 
                new Menu {
                    ItemId = 1,
                    ItemName = "Italian Pizza",
                    ItemPrice = 200,
                    RestaurantId = 2,
                    CuisineId = 6
                } };

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Menu, bool>>>())).Returns(menu.AsQueryable().BuildMock().Object);

            int itemPrice = await _menuRepository.GetItemPriceByItemId(givenItemId);

            Assert.Equal(itemPrice, expectedItemPrice);
        }

        [Fact]
        public async Task AddMenuItem_Test()
        {
            var menu = new Menu();
            await _menuRepository.AddMenuItem(menu);
            _dataRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Menu>()), Times.Once);
        }

        [Fact]
        public async Task DeleteMenu_Test()
        {
            var givenItemId = 1;
            var item = new Menu();
            _dataRepositoryMock.Setup(x => x.Find<Menu>(givenItemId)).Returns(Task.FromResult(item));
            await _menuRepository.DeleteMenu(givenItemId);
            _dataRepositoryMock.Verify(x => x.Remove(It.IsAny<Menu>()), Times.Once);
        }

        [Fact]
        public async Task DeleteMenuByRestaurantId_Test()
        {
            var givenRestaurantId = 1;
            var menuList = new List<Menu>();
            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Menu, bool>>>())).Returns(menuList.AsQueryable().BuildMock().Object);

            await _menuRepository.DeleteMenuByRestaurantId(givenRestaurantId);
            _dataRepositoryMock.Verify(x => x.Remove(menuList), Times.Exactly(menuList.Count));
        }

        [Fact]
       public async Task GetMenuByRestIdAndCuisineId_IsNotNull()
        {
            var givenRestaurantId = 1;
            var givenCuisineId = 1;
            var menuList = new List<Menu> {
                new Menu
                {
                    ItemId = 1
                }
            };
            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Menu, bool>>>())).Returns(menuList.AsQueryable().BuildMock().Object);

            await _menuRepository.GetMenuByRestIdAndCuisineId(givenRestaurantId, givenCuisineId);
            Assert.NotNull(menuList);
        }

        [Fact]
        public async Task GetMenuNameByItemId_IsEqual()
        {
            var givenItemId = 1;
            var expectedItemName = "Italian Pizza";
            var menu = new List<Menu> {
                new Menu {
                    ItemId = 1,
                    ItemName = "Italian Pizza",
                    ItemPrice = 200,
                    RestaurantId = 2,
                    CuisineId = 6
                } };

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Menu, bool>>>())).Returns(menu.AsQueryable().BuildMock().Object);

            string itemName = await _menuRepository.GetMenuNameByItemId(givenItemId);

            Assert.Equal(itemName, expectedItemName);
        }
    }
}
