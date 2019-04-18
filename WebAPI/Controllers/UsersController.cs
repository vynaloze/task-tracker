﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Model;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Service.Users;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            try
            {
                var user = await _userService.GetUser(id);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        // POST api/users
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("Object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var created = await _userService.RegisterUser(user);
                if (created == null)
                {
                    return BadRequest("Duplicate Email");
                }

                return CreatedAtRoute("", new {id = created.Id}, created);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("Object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var updated = await _userService.SaveUserData(id, user);
                if (!updated)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _userService.DeleteUser(id);
                if (!deleted)
                {
                    return NotFound();
                }

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