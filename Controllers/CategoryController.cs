using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoListAPI.Models;
using ToDoListAPI.Models.DTOs;
using ToDoListAPI.Services;

namespace ToDoListAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // **GET by ID** (already implemented)
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            return Ok(await _categoryService.GetCategoryById(id));
        }

        // **POST (Create)**
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category newCategory)
        {
            var createdCategory = await _categoryService.AddCategory(newCategory);
            return CreatedAtRoute("GetCategoryById", new { id = createdCategory.Id }, createdCategory);
        }

        // **PUT (Update)**
        [HttpPut]
        public async Task<ActionResult<Category>> UpdateCategory( Category updateCategory)
        {
            try
            {
                var updatedCategory = await _categoryService.UpdateCategory(updateCategory);
                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        // **DELETE**
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteCategory(id);
                return NoContent(); // No content to return on successful deletion
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
