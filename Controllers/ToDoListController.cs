using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Data;
using ToDoListAPI.Models;
using ToDoListAPI.Models.DTOs;
using ToDoListAPI.Services;

namespace ToDoListAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoListController : ControllerBase
    {

        /* private readonly DataContext _context;

        public ToDoListController(DataContext context)
        {
        _context = context;
        } */
        private readonly TodoListService _todoListService;

        public ToDoListController(TodoListService todoListService)
        {
            _todoListService = todoListService;
        }


        // !GET
        [HttpGet(Name = "GetAllTodoList")]
        public async Task<ActionResult<List<TodoList>>> GetAllToDoList()
        {
            return Ok(await _todoListService.GetAllTodoLists());
        }


        // !GET by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoList>> GetTodoItemById(int id)
        {
            return Ok(await _todoListService.GetTodoListById(id));
        }


        // !POST
        [HttpPost]
        public async Task<ActionResult<List<TodoList>>> AddTodoItem(TodoList newTodoItem)
        {
            return Ok(await _todoListService.AddTodoList(newTodoItem));
        }

        // !PUT
        [HttpPut]
        public async Task<ActionResult<TodoList>> UpdateTodoItem(TodoList updateTodoItem)
        {
            return Ok(await _todoListService.UpdateTodoList(updateTodoItem));
        }



        // !Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItemById(int id)
        {
            try
            {
                await _todoListService.DeleteTodoList(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }


            /* public async Task<ActionResult<List<TodoList>>> GetAllToDoList()
            {
                var TodoLists = new List<TodoList>
                {
                    new TodoList
                    {
                        Id = 0,
                        Name = "Todo1",
                        StartDate= DateTime.Today,
                        EndDate= new DateTime(2024, 6, 16),
                        Status = 0,
                        category = new Category{
                            Id = 0,
                            Name = "Acadamy"
                        }
                  }
                };
                return Ok(TodoLists);
            } */

        }
        [HttpPost("Create")]
        public async Task<ActionResult<List<Category>>> CreateTodoItem(CreateTodoItemDto newTodoItem)
        {
            return Ok(await _todoListService.CreateTodoItem(newTodoItem));
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Category>> PatchTodoItem(int id, [FromBody] JsonPatchDocument<PatchTodoItemDto> patchDocument)
        {
            return Ok(await _todoListService.PatchTodoItem(id, patchDocument, ModelState));
        }
    }
}