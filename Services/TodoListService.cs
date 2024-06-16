using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ToDoListAPI.Data;
using ToDoListAPI.Models;
using ToDoListAPI.Models.DTOs;

namespace ToDoListAPI.Services
{
    public class TodoListService
    {
        private readonly DataContext _context;

        public TodoListService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<TodoList>> GetAllTodoLists()
        {
            return await _context.TodoLists.Include(t => t.category).ToListAsync();
        }

        public async Task<TodoList> AddTodoList(TodoList newTodoList)
        {
            _context.TodoLists.Add(newTodoList);
            await _context.SaveChangesAsync();
            return newTodoList;
        }

        public async Task<TodoList?> GetTodoListById(int id)
        {
            var todoItem = await _context.TodoLists.FindAsync(id);
            return todoItem;
        }

        public async Task<TodoList> UpdateTodoList(TodoList updateTodoItem)
        {
            var existingTodoList = await _context.TodoLists.FindAsync(updateTodoItem.Id);
            if (existingTodoList == null)
            {
                throw new InvalidOperationException($"Not Found TodoItem ID :'{updateTodoItem.Id}'");
            }

            existingTodoList.Name = updateTodoItem.Name;
            existingTodoList.StartDate = updateTodoItem.StartDate;
            existingTodoList.EndDate = updateTodoItem.EndDate;
            existingTodoList.Status = updateTodoItem.Status;
            existingTodoList.category = updateTodoItem.category;
            await _context.SaveChangesAsync();
            return existingTodoList;
        }

        public async Task DeleteTodoList(int id)
        {
            var todoListToDelete = await _context.TodoLists.FindAsync(id);
            if (todoListToDelete == null)
            {
                throw new InvalidOperationException($"Not Found TodoItem ID :'{id}'");
            }

            _context.TodoLists.Remove(todoListToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TodoList>> CreateTodoItem(CreateTodoItemDto newTodoItem)
        {
            // Map DTO to TodoList (แปลง DTO เป็น TodoList)
            var todoList = new TodoList
            {
                Name = newTodoItem.Name,
                StartDate = newTodoItem.StartDate,
                EndDate = newTodoItem.EndDate,
                Status = newTodoItem.Status,
            };
            if (newTodoItem.category != null)
            {
                // ดึง Category ที่มีอยู่จาก database (ถ้ามี)
                todoList.category = await _context.Categories.FindAsync(newTodoItem.category.Id);

                if (todoList.category == null) // ถ้าไม่พบ Category ใน database
                {
                    // สร้าง Category ใหม่
                    todoList.category = new Category { Id = newTodoItem.category.Id, Name = newTodoItem.category.Name };
                }
            }
            _context.TodoLists.Add(todoList);
            await _context.SaveChangesAsync();

            return await _context.TodoLists.Include(t => t.category).ToListAsync();
        }

        public async Task<TodoList> PatchTodoItem(int id, [FromBody] JsonPatchDocument<PatchTodoItemDto> patchDocument, ModelStateDictionary modelState)
        {
            if (patchDocument == null || id <= 0)
            {
                throw new ArgumentException("Invalid patch document or ID.");
            }

            var existingTodoList = await _context.TodoLists
                .Include(t => t.category) // Eager load Category
                .FirstOrDefaultAsync(todo => todo.Id == id);

            if (existingTodoList == null)
            {
                throw new KeyNotFoundException($"TodoItem with ID '{id}' not found.");
            }

            var todoItemDto = new PatchTodoItemDto
            {
                Id = existingTodoList.Id,
                Name = existingTodoList.Name,
                StartDate = existingTodoList.StartDate,
                EndDate = existingTodoList.EndDate,
                Status = existingTodoList.Status,
                category = existingTodoList.category // ใช้ Category object โดยตรง
            };

            patchDocument.ApplyTo(todoItemDto, modelState);

            if (!modelState.IsValid)
            {
                return null; // หรือ return BadRequest(modelState)
            }

            // Update existingTodoList based on todoItemDto
            existingTodoList.Name = todoItemDto.Name;
            existingTodoList.StartDate = todoItemDto.StartDate;
            existingTodoList.EndDate = todoItemDto.EndDate;
            existingTodoList.Status = todoItemDto.Status;
            existingTodoList.category = todoItemDto.category;

            // Handle Category update
            if (todoItemDto.category != null)
            {
                // If category ID is provided, fetch the category from the database
                if (todoItemDto.category.Id != 0)
                {
                    existingTodoList.category = await _context.Categories.FindAsync(todoItemDto.category.Id);
                    if (existingTodoList.category == null)
                    {
                        throw new KeyNotFoundException($"Category with ID '{todoItemDto.category.Id}' not found.");
                    }
                }
                // If category name is provided (and ID is 0), create a new category
                else if (!string.IsNullOrEmpty(todoItemDto.category.Name))
                {
                    existingTodoList.category = new Category { Name = todoItemDto.category.Name };
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.TodoLists.Any(e => e.Id == id))
                {
                    return null; // Or throw a suitable exception
                }
                else
                {
                    throw;  // Rethrow the exception to be handled higher up
                }
            }

            return existingTodoList;
        }

    }

}