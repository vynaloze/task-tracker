using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Model;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Service.Associations;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssociationsController : ControllerBase
    {
        private readonly IAssociationService _associationService;

        public AssociationsController(IAssociationService associationService)
        {
            _associationService = associationService;
        }

        // GET api/Associations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Association>>> Get()
        {
            var associations = await _associationService.Get();
            return Ok(associations);
        }

        // GET api/Associations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Association>> Get(int id)
        {
            try
            {
                var association = await _associationService.Get(id);
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

        // POST api/Associations/ToDo/5?user=2&project=1
        [HttpPost("ToDo/{todoId}")]
        public async Task<IActionResult> Post(int todoId, int? user, int? project)
        {
            try
            {
                var association = await _associationService.Create(todoId, user, project);
                return CreatedAtRoute("", new {id = association.Id}, association);
            }
            catch (DuplicateNameException ex)
            {
                return BadRequest(ex.Message + " Use PATCH to modify it");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        // PATCH api/Associations/ToDo/5?user=2&project=1
        [HttpPatch("ToDo/{todoId}")]
        public async Task<IActionResult> Put(int todoId, int? user, int? project)
        {
            try
            {
                await _associationService.Update(todoId, user, project);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE api/Associations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _associationService.Delete(id);
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