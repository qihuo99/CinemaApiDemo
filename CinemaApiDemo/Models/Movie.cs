using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApiDemo.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Name cannot be null or empty")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Language cannot be null or empty")]
        public string Language { get; set; }

        [Required(ErrorMessage = "Rating cannot be null or empty")]
        public double Rating { get; set; }

        [NotMapped]  //image is not a valid data type in db
        public IFormFile Image { get; set; }

        public string ImageUrl { get; set; }

    }
}
