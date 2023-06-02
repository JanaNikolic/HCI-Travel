﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Model
{
    public class Smestaj
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Stars { get; set; }

        public Smestaj() { }

        public Smestaj(string name, string address, double stars)
        {
            Name = name;
            Address = address;
            Stars = stars;
        }
    }

}