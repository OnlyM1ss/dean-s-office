using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contoso.Models;
using Contoso.Repository;

namespace Contoso.Service.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets all customers. 
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _repository.GetAsync());
        }

        /// <summary>
        /// Gets the customer with the given id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var user = await _repository.GetAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(user);
            }
        }

        /// <summary>
        /// Gets all user with a data field matching the start of the given string.
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> Search(string value)
        {
            return Ok(await _repository.GetAsync(value));
        }

        /// <summary>
        /// Adds a new user or updates an existing one.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            return Ok(await _repository.UpsertAsync(user));
        }

        /// <summary>
        /// Deletes a user and all data associated with them.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repository.DeleteAsync(id);
            return Ok();
        }
    }
}