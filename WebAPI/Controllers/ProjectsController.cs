using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Model;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectsController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        // GET api/Projects
        [HttpGet]
        public ActionResult<IEnumerable<Project>> Get()
        {
            return Ok(_projectRepository.GetProjects());
        }

        // GET api/Projects/5
        [HttpGet("{id}")]
        public ActionResult<Project> Get(int id)
        {
            try
            {
                var project = _projectRepository.GetProject(id);
                if (project == null)
                {
                    return NotFound();
                }

                return Ok(project);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        // POST api/Projects
        [HttpPost]
        public IActionResult Post([FromBody] Project project)
        {
            try
            {
                if (project == null)
                {
                    return BadRequest("Object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                _projectRepository.InsertProject(project);

                return CreatedAtRoute("", new {id = project.Id}, project);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/Projects/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Project project)
        {
            try
            {
                if (project == null)
                {
                    return BadRequest("Object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var oldProject = _projectRepository.GetProject(id);
                if (oldProject == null)
                {
                    return NotFound();
                }

                _projectRepository.UpdateProject(oldProject, project);

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE api/Projects/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var oldProject = _projectRepository.GetProject(id);
                if (oldProject == null)
                {
                    return NotFound();
                }

                _projectRepository.DeleteProject(id);

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