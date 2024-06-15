using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ToDoListAPI.Models;

namespace ToDoListAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoListController : ControllerBase
    {
        [HttpGet(Name ="GetAllTodoListSample")]
        public async Task<ActionResult<List<TodoList>>> GetAllToDoList()
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
        }
    }
}