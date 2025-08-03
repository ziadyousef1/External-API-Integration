using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using External_API_Integration.Data;
using External_API_Integration.DTOs;
using External_API_Integration.Model;

namespace External_API_Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles ="ADMIN")]
    public class TodosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TodosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<TodoDto>>> GetAll() // What if we get a lot of todos? use pagination
        {
            var todos = await _context.Todos.ToListAsync();

            if (todos == null || todos.Count == 0)
            {
                return NotFound("No todos found");
            }

            List<TodoDto> todoDtos = todos.Select(todo => new TodoDto
            {
                Title = todo.Title,
                Description = todo.Description,
                Status = todo.Status,
                CreatedDate = todo.CreatedDate,
                DueDate = todo.DueDate,
                Priority = todo.Priority.ToString()
            }).ToList();

            return Ok(todoDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoDto>> GetById(int id)
        {
            var todo = await _context.Todos.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            TodoDto todoDto = new TodoDto
            {
                Title = todo.Title,
                Description = todo.Description,
                Status = todo.Status,
                CreatedDate = todo.CreatedDate,
                DueDate = todo.DueDate,
                Priority = todo.Priority.ToString()
            };

            return todoDto;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Todo>> Create(CreateTodoDto todoDto)
        {
            var todo = new Todo
            {
                Title = todoDto.Title,
                Description = todoDto.Description,
                Status = todoDto.Status,
                CreatedDate = DateTime.UtcNow, 
                DueDate = todoDto.DueDate,
                Priority = todoDto.Priority
            };

            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}