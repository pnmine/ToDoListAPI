using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListAPI.Models;

namespace ToDoListAPI.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategories();
        Task<Category?> GetCategoryById(int id);
        Task<Category> AddCategory(Category newCategory);
        Task<Category> UpdateCategory(Category updateCategory);
        Task<bool> DeleteCategory(int id);
    }
}