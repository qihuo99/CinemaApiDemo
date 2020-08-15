using CinemaApiDemo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApiDemo.Data
{
    public class CinemaDBContext : DbContext
    {
        public CinemaDBContext(DbContextOptions<CinemaDBContext> options) : base(options)
        {
            

        }

        public DbSet<Movie> Movies { get; set; }

    }
}
