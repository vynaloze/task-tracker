using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Model;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.ToDos;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDosController : ControllerBase
    {
        private readonly IToDoService _toDoService;

        public ToDosController(IToDoService doService)
        {
            _toDoService = doService;
        }

        // GET api/ToDos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDo>>> Get()
        {
            var todos = await _toDoService.Get();
            return Ok(todos);
        }

        // GET api/ToDos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDo>> Get(int id)
        {
            try
            {
                var todo = await _toDoService.Get(id);
                if (todo == null)
                {
                    return NotFound();
                }

                return Ok(todo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        // POST api/ToDos
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ToDo toDo)
        {
            try
            {
                if (toDo == null)
                {
                    return BadRequest("Object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var created = await _toDoService.Create(toDo);

                return CreatedAtRoute("", new {id = created.Id}, toDo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/ToDos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ToDo toDo)
        {
            try
            {
                if (toDo == null)
                {
                    return BadRequest("Object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var oldTodo = await _toDoService.Get(id);
                if (oldTodo == null)
                {
                    return NotFound();
                }

                await _toDoService.Update(oldTodo, toDo);

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE api/ToDos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var oldTodo = await _toDoService.Get(id);
                if (oldTodo == null)
                {
                    return NotFound();
                }

                await _toDoService.Delete(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        // PATCH api/ToDos/5/WorkingTime/{start_ts}/{end_ts}
        [HttpPatch("{id}/WorkingTime/{start_ts}/{end_ts}")]
        public async Task<IActionResult> WorkingTime(int id, long start_ts, long end_ts)
        {
            try
            {
                var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                var start = dtDateTime.AddSeconds(start_ts).ToLocalTime();
                var end = dtDateTime.AddSeconds(end_ts).ToLocalTime();
                await _toDoService.SetWorkingTime(id, start, end);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }
        
        // PATCH api/ToDos/5/User/{userId}
        [HttpPatch("{id}/User/{userId}")]
        public async Task<IActionResult> User(int id, int? userId)
        {
            try
            {
                await _toDoService.AssignToUser(id, userId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }
        
        // PATCH api/ToDos/5/User
        [HttpPatch("{id}/User/")]
        public async Task<IActionResult> ClearUser(int id)
        {
            try
            {
                await _toDoService.AssignToUser(id, null);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }
        
        // PATCH api/ToDos/5/Project/{projectId}
        [HttpPatch("{id}/Project/{projectId}")]
        public async Task<IActionResult> Project(int id, int? projectId)
        {
            try
            {
                await _toDoService.AssociateWithProject(id, projectId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }
        
        // PATCH api/ToDos/5/Project
        [HttpPatch("{id}/Project")]
        public async Task<IActionResult> ClearProject(int id)
        {
            try
            {
                await _toDoService.AssociateWithProject(id, null);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }
    }
}