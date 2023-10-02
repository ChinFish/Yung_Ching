using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _todoContext;  
        public TodoItemsController(TodoContext todoContext) 
        { 
            _todoContext = todoContext;   
        }
        // GET: api/<TodoItemsController>
        [HttpGet]
        public ActionResult<IEnumerable<TodoItem>> Get()
        {
            return _todoContext.TodoItems;
        }

        // GET api/<TodoItemsController>/5
        [HttpGet("{id}")]
        public ActionResult<TodoItem> Get(Guid id)
        {
            var result = _todoContext.TodoItems.Find(id);
            if (result == null)
            {
                return NotFound("Empty data");  
            }    

            return result;
        }

        // POST api/<TodoItemsController>
        [HttpPost]
        public ActionResult<TodoItem> Post([FromBody] TodoItem value) 
        {
            _todoContext.TodoItems.Add(value);
            _todoContext.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = value.Id }, value);
        }

        // PUT api/<TodoItemsController>/5
        [HttpPut("{id}")]
        public ActionResult Put (Guid id, [FromBody] TodoItem value)
        {
            if (id != value.Id)
            {
                return BadRequest();
            }

            _todoContext.Entry(value).State = EntityState.Modified;

            try
            {
                _todoContext.SaveChanges();
            }
            catch(DbUpdateException) 
            {
                if (_todoContext.TodoItems.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500, "Access Error!");
                }
            }
            return NoContent(); 

        }

        // DELETE api/<TodoItemsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
