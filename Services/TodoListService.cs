using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Data;
using ToDoListAPI.Models;

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
            return await _context.TodoLists.ToListAsync();
        }

        public async Task<TodoList?> GetTodoListById(int id)
        {
            return await _context.TodoLists.FindAsync(id);
        }

        public async Task<TodoList> AddTodoList(TodoList newTodoList)
        {
            _context.TodoLists.Add(newTodoList);
            await _context.SaveChangesAsync();
            return newTodoList;
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
    }

}