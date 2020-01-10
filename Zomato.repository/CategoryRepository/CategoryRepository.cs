using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Data;
using Zomato.DomainModel.Models;
using Zomato.Repository.DataRepository;

namespace Zomato.Repository.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        private IDataRepository _dataRepository;

        public CategoryRepository(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public async Task<List<Category>> CategoryList()
        {
            return await _dataRepository.Get<Category>();
        }

        public async Task<string> GetCategoryById(int categoryId)
        {
            var category = await _dataRepository.Where<Category>(x => x.CategoryId == categoryId).FirstAsync();
            return category.CategoryName;
        }

        public async Task<int> GetCategoryIdByName(string categoryName)
        {
            var category = await _dataRepository.Where<Category>(x => x.CategoryName == categoryName).FirstOrDefaultAsync();
            return category.CategoryId;
        }
    }
}
