using System;
using System.Collections.Generic;
using DataAccess.Model;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDosController : ControllerBase
    {
        private readonly IToDoRepository _toDoRepository;

        public ToDosController(IToDoRepository toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }

        // GET api/ToDos
        [HttpGet]
        public ActionResult<IEnumerable<ToDo>> Get()
        {
            return Ok(_toDoRepository.GetToDos());
        }

        // GET api/ToDos/5
        [HttpGet("{id}")]
        public ActionResult<ToDo> Get(int id)
        {
            try
            {
                var task = _toDoRepository.GetToDo(id);
                if (task == null)
                {
                    return NotFound();
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        // POST api/ToDos
        [HttpPost]
        public IActionResult Post([FromBody] ToDo toDo)
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

                _toDoRepository.InsertTodo(toDo);

                return CreatedAtRoute("", new {id = toDo.Id}, toDo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/ToDos/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ToDo toDo)
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

                var oldTask = _toDoRepository.GetToDo(id);
                if (oldTask == null)
                {
                    return NotFound();
                }

                _toDoRepository.UpdateTodo(oldTask, toDo);

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
        public IActionResult Delete(int id)
        {
            try
            {
                var oldTask = _toDoRepository.GetToDo(id);
                if (oldTask == null)
                {
                    return NotFound();
                }

                _toDoRepository.DeleteTodo(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }
    }
}