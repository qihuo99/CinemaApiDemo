using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CinemaApiDemo.Data;
using CinemaApiDemo.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesFuntionDemoController : ControllerBase
    {
        private CinemaDBContext _dbContext;

        public MoviesFuntionDemoController(CinemaDBContext dbContext)
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

        //demo how to implement attribute routing
        //api/Movies/Test/id
        [HttpGet("[action]/{id}")]
        public int Test(int id)
        {
            return id;
        }


        //// POST api/<MoviesController>
        //[HttpPost]
        //public IActionResult Post([FromBody] Movie movieObj)
        //{
        //    _dbContext.Movies.Add(movieObj);
        //    _dbContext.SaveChanges();
        //    return StatusCode(StatusCodes.Status201Created);

        //    //this is the json format
        //    //{
        //    //    "Name":"The Speedy 2",
        //    //    "Language":"English",
        //    //    "Rating": 6,
        //    //}
        //}

        [HttpPost]
        public IActionResult Post([FromForm] Movie movieObj)
        {
            //generate a new unique image id to avoid duplicate
            var guid = Guid.NewGuid();
            var filePath = Path.Combine("wwwroot\\image", guid + ".jpg");
            if (movieObj.Image != null)
            {
                var fileStream = new FileStream(filePath, FileMode.Create);
                movieObj.Image.CopyTo(fileStream); //save the image file url
            }
            movieObj.ImageUrl = filePath.Remove(0, 7); //remove wwwroot
            _dbContext.Movies.Add(movieObj);
            _dbContext.SaveChanges();

            return Ok();
        }

        // PUT api/<MoviesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm] Movie movieObj)
        {
            var movie = _dbContext.Movies.Find(id);
            if (movie == null)
            {
                return NotFound("No Record Found for this Id!");
            }
            else
            {
                var guid = Guid.NewGuid();
                var filePath = Path.Combine("wwwroot\\image", guid + ".jpg");
                if (movieObj.Image != null)
                {
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    movieObj.Image.CopyTo(fileStream); //save the image file url
                    movieObj.ImageUrl = filePath.Remove(0, 7); //remove wwwroot
                }

                movie.Name = movieObj.Name;
                movie.Language = movieObj.Language;
                movie.Rating = movieObj.Rating;
                movie.ImageUrl = movieObj.ImageUrl;
                _dbContext.SaveChanges();
                return Ok("Record updated successfully!");    //return success code with message
            }
        }

        // DELETE api/<MoviesController>/5
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
