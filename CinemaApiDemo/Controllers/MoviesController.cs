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
    public class MoviesController : ControllerBase
    {
        private CinemaDBContext _dbContext;

        public MoviesController(CinemaDBContext dbContext)
        {
            _dbContext = dbContext;

        }

        // GET: api/<MoviesController>
        [HttpGet]
        public IEnumerable<Movie> Get()
        {

            return _dbContext.Movies;
        }

        // GET api/<MoviesController>/5
        [HttpGet("{id}")]
        public Movie Get(int id)
        { 
            var movie = _dbContext.Movies.Find(id);
            return movie;
        }

        // POST api/<MoviesController>
        [HttpPost]
        public IActionResult Post([FromBody] Movie movieObj)
        {
            _dbContext.Movies.Add(movieObj);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
           
            //this is the json format
            //{
            //    "Name":"The Speedy 2",
            //    "Language":"English",
            //    "Rating": 6,
            //}
        }

        // PUT api/<MoviesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Movie movieObj)
        {
            var movie = _dbContext.Movies.Find(id);
            movie.Name = movieObj.Name;
            movie.Language = movieObj.Language;
            movie.Rating = movieObj.Rating;
            _dbContext.SaveChanges();
        }

        // DELETE api/<MoviesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var movie = _dbContext.Movies.Find(id);
            _dbContext.Movies.Remove(movie);
            _dbContext.SaveChanges();
        }
    }
}
