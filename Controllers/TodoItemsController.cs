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
        //Constructor
        public TodoItemsController(TodoContext todoContext) 
        {
        //Get data from database
            _todoContext = todoContext;
        }

        [HttpGet()]
        public IEnumerable<TodoItem> Get()
        {
            var result = _todoContext.TodoItems;

            //return _todoContext.TodoItems;
            return result;
        }

        //Implement get data from certain ID
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

        //Implement search certain name, return all of correct result
        [HttpGet("Name={Name}")]
        public IEnumerable<TodoItem> Get(string name)
        {
            var result = _todoContext.TodoItems.Where(a => a.Name == name);

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

        // PUT api/<TodoItemsController>/id
        [HttpPut("{id}")]
        public ActionResult Put (Guid id, [FromBody] TodoItem value)
        {
            //Check if search id is not match
            if (id != value.Id)
            {
                return BadRequest();
            }

            _todoContext.Entry(value).State = EntityState.Modified;

            //Avoid modified failed
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

        // DELETE api/<TodoItemsController>/id
        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id, [FromBody] TodoItem value)
        {
            var delete = _todoContext.TodoItems.Find(id);
            if(delete==null)
            {
                return NotFound();
            }
            _todoContext.TodoItems.Remove(delete);
            _todoContext.SaveChanges();
            return NoContent();
        }
    }
}
