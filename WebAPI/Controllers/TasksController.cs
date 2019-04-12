using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Model;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Task = DataAccess.Model.Task;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TasksController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        // GET api/Tasks
        [HttpGet]
        public ActionResult<IEnumerable<Task>> Get()
        {
            return Ok(_taskRepository.GetTasks());
        }

        // GET api/Tasks/5
        [HttpGet("{id}")]
        public ActionResult<Task> Get(int id)
        {
            try
            {
                var task = _taskRepository.GetTask(id);
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

        // POST api/Tasks
        [HttpPost]
        public IActionResult Post([FromBody] Task task)
        {
            try
            {
                if (task == null)
                {
                    return BadRequest("Object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                _taskRepository.InsertTask(task);

                return CreatedAtRoute("", new {id = task.Id}, task);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/Tasks/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Task task)
        {
            try
            {
                if (task == null)
                {
                    return BadRequest("Object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var oldTask = _taskRepository.GetTask(id);
                if (oldTask == null)
                {
                    return NotFound();
                }

                _taskRepository.UpdateTask(oldTask, task);

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE api/Tasks/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var oldTask = _taskRepository.GetTask(id);
                if (oldTask == null)
                {
                    return NotFound();
                }

                _taskRepository.DeleteTask(id);

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