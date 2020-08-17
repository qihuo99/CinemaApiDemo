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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<SamplesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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
