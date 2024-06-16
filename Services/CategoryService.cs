using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Data;
using ToDoListAPI.Models;

namespace ToDoListAPI.Services
{
    public class CategoryService
    {
        private readonly DataContext _context;

        public CategoryService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> AddCategory(Category newCategory)
        {
            // Validate the category name
            if (string.IsNullOrWhiteSpace(newCategory.Name))
            {
                throw new ArgumentException("Category name cannot be empty.");
            }

            if (await _context.Categories.AnyAsync(c => c.Name.ToLower() == newCategory.Name.ToLower()))
            {
                throw new InvalidOperationException("A category with this name already exists.");
            }

            try
            {
                _context.Categories.Add(newCategory);
                await _context.SaveChangesAsync();
                return newCategory;
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("An error occurred while adding the category. Please try again later.");
            }
        }


        public async Task<Category> UpdateCategory(Category updateCategory)
        {
            var existingCategory = await _context.Categories.FindAsync(updateCategory.Id);
            if (existingCategory == null)
            {
                throw new Exception($"Category not found with ID: {updateCategory.Id}");
            }

            existingCategory.Name = updateCategory.Name;
            await _context.SaveChangesAsync();
            return existingCategory;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var categoryToDelete = await _context.Categories.FindAsync(id);
            if (categoryToDelete == null)
            {
                throw new Exception($"Category not found with ID: {id}");
            }

            // Check if any todo items are using this category
            if (await _context.TodoLists.AnyAsync(todo => todo.category != null && todo.category.Id == id))
            {
                throw new InvalidOperationException("Cannot delete category because there are todo items associated with it.");
            }

            _context.Categories.Remove(categoryToDelete);
            await _context.SaveChangesAsync();

            return true; // Indicate successful deletion
        }
    }

}