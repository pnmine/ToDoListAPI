using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListAPI.Models;
using ToDoListAPI.Models.DTOs;

namespace ToDoListAPI.Services
{
    public interface ITodoListService
    {
        Task<List<TodoList>> GetAllTodoLists();
        //Just Example
        Task<TodoList> AddTodoList(TodoList newTodoList);
        Task<TodoList?> GetTodoListById(int id);
        Task<TodoList> UpdateTodoList(TodoList updateTodoItem);
        Task DeleteTodoList(int id);
        //Use this to Crate TodoItem
        Task<List<TodoList>> CreateTodoItem(CreateTodoItemDto newTodoItem);

    }
}