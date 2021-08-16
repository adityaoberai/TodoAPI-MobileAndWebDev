using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Data;
using TodoAPI.Models;
using TodoAPI.Models.DTO;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TodoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult> GetTodos()
        {
            TodoListResponse response;
            try
            {
                List<Todo> todoList = await _context.Todos.ToListAsync();

                if (todoList.Count == 0)
                {
                    response = new TodoListResponse
                    {
                        Status = HttpStatusCode.NotFound,
                        Message = "No todos found"
                    };
                    return NotFound(response);
                }

                response = new TodoListResponse
                {
                    Status = HttpStatusCode.OK,
                    Message = "Got list of todos successfully",
                    Response = todoList
                };
                return Ok(response);
            }
            catch(Exception ex)
            {
                response = new TodoListResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
                return BadRequest(response);
            }
        }

        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetTodo(int id)
        {
            TodoResponse response;
            try 
            { 
                var todo = await _context.Todos.FindAsync(id);

                if (todo == null)
                {
                    response = new TodoResponse
                    {
                        Status = HttpStatusCode.NotFound,
                        Message = "No todo found at the entered Id"
                    };
                    return NotFound(response);
                }

                response = new TodoResponse
                {
                    Status = HttpStatusCode.OK,
                    Message = "Got todo successfully",
                    Response = todo
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new TodoResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
                return BadRequest(response);
            }
        }

        // PUT: api/Todo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult> PutTodo(int id, TodoDTO todoDTO)
        {
            TodoResponse response;
            try
            {
                var todo = DTOToModel(todoDTO, id);
                _context.Entry(todo).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoExists(id))
                    {
                        response = new TodoResponse
                        {
                            Status = HttpStatusCode.NotFound,
                            Message = "No todo found at the entered Id"
                        };
                        return NotFound(response);
                    }
                    else
                    {
                        throw;
                    }
                }

                response = new TodoResponse
                {
                    Status = HttpStatusCode.OK,
                    Message = "Updated todo successfully",
                    Response = todo
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new TodoResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
                return BadRequest(response);
            }
        }

        // POST: api/Todo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostTodo(TodoDTO todoDTO)
        {
            TodoResponse response;
            try
            {
                var todo = DTOToModel(todoDTO);
                _context.Todos.Add(todo);
                await _context.SaveChangesAsync();

                response = new TodoResponse
                {
                    Status = HttpStatusCode.Created,
                    Message = "Created todo successfully",
                    Response = todo
                };
                return new CreatedResult("TodoDB",response);
            }
            catch (Exception ex)
            {
                response = new TodoResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
                return BadRequest(response);
            }
        }

        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodo(int id)
        {
            TodoResponse response;
            try 
            { 
                var todo = await _context.Todos.FindAsync(id);
                if (todo == null)
                {
                    response = new TodoResponse
                    {
                        Status = HttpStatusCode.NotFound,
                        Message = "No todo found at the entered Id"
                    };
                    return NotFound(response);
                }

                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();

                response = new TodoResponse
                {
                    Status = HttpStatusCode.OK,
                    Message = "Deleted todo successfully",
                    Response = todo
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new TodoResponse
                {
                    Status = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
                return BadRequest(response);
            }
        }

        private bool TodoExists(int id)
        {
            return _context.Todos.Any(e => e.Id == id);
        }

        private Todo DTOToModel(TodoDTO todo, int id = 0)
        {
            return new Todo { Id = id, Name = todo.Name, Completed = todo.Completed };
        }
    }
}
