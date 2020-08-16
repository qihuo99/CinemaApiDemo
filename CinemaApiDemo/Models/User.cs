using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApiDemo.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        //use a collection of property of types
        //Build a one-to-many relationship between user and reservation entities
        public ICollection<Reservation> Reservations { get; set; }
    }
}
