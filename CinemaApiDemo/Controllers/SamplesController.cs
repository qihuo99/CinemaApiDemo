using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  //=> put on top of controller will protect all methods
    public class SamplesController : ControllerBase
    {
        //The samples controller is created to demo
        //how to protect routes from being easily hacked.

        // GET: api/<SamplesController>
        [HttpGet]
        public string Get()
        {
            return "Hello From the User Side";
            //return new string[] { "value1", "value2" };
        }

        //By default, only the first Get method will be accessed
        //Thus, we need to specify a particular Authorize method here
        [Authorize(Roles = "Admin")]
        // GET api/<SamplesController>/3
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "Hello From the Admin Side";
        }

        // POST api/<SamplesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SamplesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SamplesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
