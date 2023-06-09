﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Model
{
    public class Restoran
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public FoodType FoodType { get; set; }

        public virtual ICollection<Aranzman> Arrangements { get; set; }

        public Restoran() { }
        public Restoran(string name, string address, FoodType foodType)
        {
            Name = name;
            Address = address;
            FoodType = foodType;
            this.Arrangements = new List<Aranzman>();
        }
    }
}
