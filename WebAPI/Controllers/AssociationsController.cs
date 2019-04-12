using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Model;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssociationsController : ControllerBase
    {
        private readonly IAssociationRepository _associationRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;

        public AssociationsController(IAssociationRepository associationRepository, ITaskRepository taskRepository,
            IUserRepository userRepository, IProjectRepository projectRepository)
        {
            _associationRepository = associationRepository;
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }

        // GET api/Associations
        [HttpGet]
        public ActionResult<IEnumerable<Association>> Get()
        {
            return Ok(_associationRepository.GetAssociations());
        }

        // GET api/Associations/5
        [HttpGet("{id}")]
        public ActionResult<Association> Get(int id)
        {
            try
            {
                var association = _associationRepository.GetAssociation(id);
                if (association == null)
                {
                    return NotFound();
                }

                return Ok(association);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        // POST api/Associations/Task/5?user=2&project=1
        [HttpPost("Task/{taskId}")]
        public IActionResult Post(int taskId, int? user, int? project)
        {
            try
            {
                var existsDuplicate = _associationRepository.GetAssociations().Any(a => a.Task.Id == taskId);
                if (existsDuplicate)
                {
                    return BadRequest("Such task is already assigned. Use PATCH to modify it");
                }
                
                var newAssociation = new Association();
                var task = _taskRepository.GetTask(taskId);
                if (task == null)
                {
                    return NotFound("Task not found");
                }

                newAssociation.Task = task;
                
                if (user.HasValue)
                {
                    var dbUser = _userRepository.GetUser(user.Value);
                    if (dbUser == null)
                    {
                        return NotFound("User not found");
                    }

                    newAssociation.User = dbUser;
                }

                if (project.HasValue)
                {
                    var dbProject = _projectRepository.GetProject(project.Value);
                    if (dbProject == null)
                    {
                        return NotFound("Project not found");
                    }

                    newAssociation.Project = dbProject;
                }

                _associationRepository.InsertAssociation(newAssociation);

                return CreatedAtRoute("", new {id = newAssociation.Id}, newAssociation);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        // PATCH api/Associations/Task/5?user=2&project=1
        [HttpPatch("Task/{taskId}")]
        public IActionResult Put(int taskId, int? user, int? project)
        {
            try
            {
                var oldAssociation = _associationRepository.GetAssociations().FirstOrDefault(a => a.Task.Id == taskId);
                if (oldAssociation == null)
                {
                    return NotFound("Association with such task not found");
                }

                var newAssociation = new Association{Task = oldAssociation.Task};
                if (user.HasValue)
                {
                    var dbUser = _userRepository.GetUser(user.Value);
                    if (dbUser == null)
                    {
                        return NotFound("User not found");
                    }

                    newAssociation.User = dbUser;
                }

                if (project.HasValue)
                {
                    var dbProject = _projectRepository.GetProject(project.Value);
                    if (dbProject == null)
                    {
                        return NotFound("Project not found");
                    }

                    newAssociation.Project = dbProject;
                }

                _associationRepository.UpdateAssociation(oldAssociation, newAssociation);

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE api/Associations/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var oldAssociation = _associationRepository.GetAssociation(id);
                if (oldAssociation == null)
                {
                    return NotFound();
                }

                _associationRepository.DeleteAssociation(id);

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