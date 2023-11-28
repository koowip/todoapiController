using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ToDoApi.Models;

namespace todoapiController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemsController : ControllerBase
    {
        private readonly ToDoContext dbContext;

        public ToDoItemsController(ToDoContext context)
        {
            dbContext = context;
        }

        // Get: api/ToDoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetToDoItems()
        {
            return await dbContext.ToDoItems.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItem>> GetToDoItem(long id)
        {
            var todoItem = await dbContext.ToDoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        //Post: api/ToDoItems
        [HttpPost]
        public async Task<ActionResult<ToDoItem>> PostToDoItems(ToDoItem todo)
        {
            var todoitem = new ToDoItem
            {
                IsComplete = todo.IsComplete,
                Content = todo.Content
            };

            dbContext.ToDoItems.Add(todoitem);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetToDoItem), new {id = todoitem.Id}, todoitem);

        } 

        //Put: api/ToDoItems/{id}
        [HttpPut]
        public async Task<IActionResult> PutToDoItem(ToDoItem todoitem)
        {

            if (!dbContext.ToDoItems.Any(i => i.Id == todoitem.Id))
                {
                    return NotFound();
                }

            dbContext.Entry(todoitem).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            return NoContent();

        }

        //Delete: api/ToDoItems/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem(long id)
        {
            var todoitem = await dbContext.ToDoItems.FindAsync(id);

            if(todoitem == null)
                return NotFound();
            
            dbContext.ToDoItems.Remove(todoitem);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
