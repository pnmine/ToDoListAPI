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
    public class TodoListService : ITodoListService
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
            var existingTodoList = await _context.TodoLists
                .Include(t => t.category)
                .FirstOrDefaultAsync(todo => todo.Id == updateTodoItem.Id);

            if (existingTodoList == null)
            {
                throw new InvalidOperationException($"Not Found TodoItem ID :'{updateTodoItem.Id}'");
            }

            existingTodoList.Name = updateTodoItem.Name;
            existingTodoList.StartDate = updateTodoItem.StartDate;
            existingTodoList.EndDate = updateTodoItem.EndDate;
            existingTodoList.Status = updateTodoItem.Status;

            if (updateTodoItem.category != null && updateTodoItem.category.Name != null)
            {
                // ตรวจสอบว่ามี Category ที่มีชื่อเดียวกันอยู่แล้วหรือไม่
                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name == updateTodoItem.category.Name);

                if (existingCategory != null)
                {
                    // ถ้ามี Category ที่มีชื่อเดียวกันอยู่แล้ว ให้นำ Category นั้นมาใช้
                    existingTodoList.category = existingCategory;
                }
                else
                {
                    // ถ้าไม่มี Category ที่มีชื่อเดียวกัน ให้สร้าง Category ใหม่
                    existingTodoList.category = new Category { Name = updateTodoItem.category.Name };
                }
            }
            else
            {
                // ถ้าไม่ได้ส่ง Category มา ให้ set Category เป็น null
                existingTodoList.category = null;
            }

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
            var todoList = new TodoList
            {
                Name = newTodoItem.Name,
                StartDate = newTodoItem.StartDate,
                EndDate = newTodoItem.EndDate,
                Status = newTodoItem.Status,
                category = new Category()
            };

            if (newTodoItem.category != null)
            {
                // Check if a category with the same name exists
                todoList.category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name == newTodoItem.category.Name);

                // If no matching category is found, create a new one.
                if (todoList.category == null)
                {
                    todoList.category = new Category { Name = newTodoItem.category.Name };
                }
            }

            _context.TodoLists.Add(todoList);
            await _context.SaveChangesAsync();

            return await _context.TodoLists.Include(t => t.category).ToListAsync();
        }

    }

}