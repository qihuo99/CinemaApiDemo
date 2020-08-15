using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaApiDemo.Data;
using CinemaApiDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieStatusCodeController : ControllerBase
    {
        private readonly CinemaDBContext _dbContext;

        public MovieStatusCodeController(CinemaDBContext dbContext)
        {
            _dbContext = dbContext;

        }

        // GET: api/<MovieStatusCodeController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_dbContext.Movies); //return success code 200
            //return BadRequest; //return 400 or bad request errors
            //return NotFound;  //return 404 or not found errors
            //return StatusCode(StatusCodes.Status200OK);
        }

        // GET api/<MovieStatusCodeController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var movie = _dbContext.Movies.Find(id);
            if (movie == null)
            {
                return NotFound("No Record Found for this Id!");
            }
            return Ok(movie); 
        }

        // POST api/<MovieStatusCodeController>
        [HttpPost]
        public IActionResult Post([FromBody] Movie movieObj)
        {
            _dbContext.Movies.Add(movieObj);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);//post is to create
        }

        // PUT api/<MovieStatusCodeController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Movie movieObj)
        {
            var movie = _dbContext.Movies.Find(id);
            if (movie == null)
            {
                return NotFound("No Record Found for this Id!");
            }
            else 
            {
                movie.Name = movieObj.Name;
                movie.Language = movieObj.Language;
                _dbContext.SaveChanges();
                return Ok("Record updated successfully!");    //return success code with message
            }
        }

        // DELETE api/<MovieStatusCodeController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var movie = _dbContext.Movies.Find(id);
            if (movie == null)
            {
                return NotFound("No Record Found for this Id!");
            }
            else
            {
                _dbContext.Movies.Remove(movie);
                _dbContext.SaveChanges();
                return Ok("Record deleted successfully!");
            }
          
        }
    }
}
