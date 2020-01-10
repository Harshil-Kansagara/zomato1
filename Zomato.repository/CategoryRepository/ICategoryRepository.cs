using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;

namespace Zomato.Repository.CategoryRepository
{
    public interface ICategoryRepository
    {
        Task<string> GetCategoryById(int categoryId);
        Task<List<Category>> CategoryList();
        Task<int> GetCategoryIdByName(string categoryName);
    }
}
