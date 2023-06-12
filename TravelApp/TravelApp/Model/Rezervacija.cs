﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Model
{
    public class Rezervacija
    {
        [Key]
        public int Id { get; set; }

        public DateTime Datum { get; set; }

        public Aranzman Aranzman { get; set; }

        public User User { get; set; }

        public Rezervacija() { }

        public Rezervacija(Aranzman aranzman, DateTime datum, User user)
        {
            Aranzman = aranzman; 
            Datum = datum;
            User = user;
        }
    }
}
