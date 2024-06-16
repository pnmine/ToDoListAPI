using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListAPI.Models.DTOs
{
    public class PatchTodoItemDto
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public Category? category { get; set; } 
    }
}

/* [
  {
    "path": "/Name",
    "op": "replace",
    "value": "test Patch"
  },
{
    "path": "/Status",
    "op": "replace",
    "value": "1"
  }
] */