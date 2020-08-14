using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaApiDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private static List<Movie> movies = new List<Movie>
        {
            new Movie(){ Id=1, Name="Mission Impossible 7",  Language="English"},
            new Movie(){ Id=2, Name="The Matrix 4",  Language="English"},
            new Movie(){ Id=3, Name="Patriot Game",  Language="English"}
        };

        [HttpGet]  //Once the web request is send, the httpget will execute the Get() method
        public IEnumerable<Movie> Get() 
        {
            return movies;
        }

        [HttpPost]
        public void Post([FromBody] Movie movie)
        {
            movies.Add(movie);

            //This is the valid json format to test in Postman for Post() method
            //{
            //   "Id":7,
            //   "Name":"Fast 9",
            //   "Language":"English"
            //}
        }


    }
}
