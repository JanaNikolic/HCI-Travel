using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        public ICollection<Kupovina> Kupovine { get; set; } = new HashSet<Kupovina>();
        public ICollection<Rezervacija> Rezervacije { get; set; } = new HashSet<Rezervacija>();

        public User(string userName, string password, Role role)
        {
            UserName = userName;
            Password = password;
            Role = role;
        }

        public User() { }
    }

    public enum Role
    {
        Agent,
        Client,
    }
}
