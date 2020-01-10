using MockQueryable.Moq;
using Moq;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Zomato.DomainModel.Models;
using Zomato.Repository.CategoryRepository;
using Zomato.Repository.DataRepository;
using Zomato.Repository.Test.Bootstrap;
using Zomato.Repository.UnitofWork;

namespace Zomato.Repository.Test.Modules.CategoryTesting
{
    [Collection("Register Dependency")]
    public class CategoryRepositoryTest
    {

        private Mock<IDataRepository> _dataRepositoryMock { get; }
        private ICategoryRepository _categoryRepository { get; }

        public CategoryRepositoryTest(Initialize initialize)
        {
            _dataRepositoryMock = initialize.serviceProvider.GetService<Mock<IDataRepository>>();
            _categoryRepository = initialize.serviceProvider.GetService<ICategoryRepository>();
        }

        [Fact]
        public async Task GetCategoryList_Equal()
        {
            List<string> categories = new List<string>{
                "Breakfast","Lunch","Dinner","Cafe","Dessert"
            };

            var categorList = new List<Category>() { 
                new Category()
                {
                    CategoryId = 1,
                    CategoryName = "Breakfast"
                }, new Category()
                {
                    CategoryId = 2,
                    CategoryName = "Lunch"
                },new Category()
                {
                    CategoryId = 3,
                    CategoryName = "Dinner"
                }, new Category()
                {
                    CategoryId = 4,
                    CategoryName = "Cafe"
                },new Category()
                {
                    CategoryId = 5,
                    CategoryName = "Dessert"
                }
               };
            
            _dataRepositoryMock.Setup(x=>x.Get<Category>()).Returns(Task.FromResult(categorList));

            var categoryData = await _categoryRepository.CategoryList();
            List<string> categories2 = new List<string>();
            foreach (var each in categoryData)
            {
                categories2.Add(each.CategoryName);
            }

            Assert.Equal(categories, categories2);
        }

        [Fact]
        public async Task GetCategoryById_Equal()
        {
            var expectedCategoryName = "Breakfast";
            var givenCategoryId = 1;
            var categorList = new List<Category>() {
                new Category()
                {
                    CategoryId = 1,
                    CategoryName = "Breakfast"
                }
               };
           
            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Category, bool>>>())).Returns(categorList.AsQueryable().BuildMock().Object);

            var categoryNameData = await _categoryRepository.GetCategoryById(givenCategoryId);

            Assert.Equal(categoryNameData, expectedCategoryName);
        }

        [Fact]
        public void GetCategoryByName_Equal()
        {
            var expectedCategoryId = 1;
            var givenCategoryName = "Breakfast";
            var categorList = new List<Category>() {
                new Category()
                {
                    CategoryId = 1,
                    CategoryName = "Breakfast"
                }
               };

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Category, bool>>>())).Returns(categorList.AsQueryable().BuildMock().Object);

            var categoryIdData = _categoryRepository.GetCategoryIdByName(givenCategoryName);

            Assert.Equal(categoryIdData.Result, expectedCategoryId);
        }
    }
}
