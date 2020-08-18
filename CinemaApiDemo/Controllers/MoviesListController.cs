using System;
using System.IO;
using CinemaApiDemo.Data;
using CinemaApiDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CinemaApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesListController : ControllerBase
    {
        private CinemaDBContext _dbContext;

        public MoviesListController(CinemaDBContext dbContext)
        {
            _dbContext = dbContext; //use to access the dbset
        }

        // GET: api/<MoviesController>
        [Authorize]
        [HttpGet("[action]")]
        public IActionResult AllMovies(string sort, int? pageNumber, int? pageSize)
        {
            //if pageNumber & pageSize are not null, take the passing parameters values
            //otherwise, take the default values 1 & 5 specified as below
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 5;
            //Since below is LINQ, need to import System.LINQ class
            var movies = from movie in _dbContext.Movies
            select new
            {
                Id = movie.Id,
                Name = movie.Name,
                Duration = movie.Duration,
                Language =  movie.Language,
                Rating = movie.Rating,
                Genre = movie.Genre,
                ImageUrl = movie.ImageUrl
            };
            ///api/MoviesList/AllMovies?pageNumber=2&pageSize=2  use local url like this
            switch (sort) //sorting in asc or desc order
            {
                case "desc":
                    return Ok(movies.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize).OrderByDescending(m=>m.Rating));
                case "asc":
                    return Ok(movies.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize).OrderBy(m => m.Rating));
                default:  //Do pagination
                    return Ok(movies.Skip((currentPageNumber - 1)* currentPageSize).Take(currentPageSize));
            }

            //use this approach only if you want to return all fields
            //var allMovies = _dbContext.Movies;
            //return Ok(allMovies);
        }

        //demo how to implement attribute routing
        //if use api/MoviesList/MovieDetail?id=1  then use=> [HttpGet("[action]")]
        //otherwise use this api/MoviesList/MovieDetail/1
        [Authorize]
        [HttpGet("[action]/{id}")]
        public IActionResult MovieDetail(int id)
        {
            var movie = _dbContext.Movies.Find(id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        //this is for searching a particular movie by name
        //use api/MoviesList/FindMovies?movieName=MissionImpossible
        [Authorize]
        [HttpGet("[action]")] //customized method needs to add action token here
        public IActionResult FindMovies(string movieName)
        {
            var movies = from movie in _dbContext.Movies
                         where movie.Name.StartsWith(movieName)
                         select new
                         {   //only Id, Name, and ImageUrl are good enough
                             Id = movie.Id,
                             Name = movie.Name,
                             ImageUrl = movie.ImageUrl
                         };
            return Ok(movies);
        }

        [Authorize(Roles = "Admin")] //adding a movie only by admin user
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

            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/<MoviesController>/5
        [Authorize(Roles = "Admin")] //only admin user can access this method
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
                movie.Description = movieObj.Description;
                movie.Language = movieObj.Language;
                movie.Duration = movieObj.Duration;
                movie.PlayingDate = movieObj.PlayingDate;
                movie.PlayingTime = movieObj.PlayingTime;
                movie.Rating = movieObj.Rating;
                movie.Genre = movieObj.Genre;
                movie.TrailorUrl = movieObj.TrailorUrl;
                movie.TicketPrice = movieObj.TicketPrice;
                movie.ImageUrl = movieObj.ImageUrl;
                _dbContext.SaveChanges();
                return Ok("Record updated successfully!");    //return success code with message
            }
        }

        // DELETE api/<MoviesController>/2
        [Authorize(Roles = "Admin")] //only admin user can access this method
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
