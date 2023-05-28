using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Model
{
    class Smestaj
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Stars stars { get; set; }
    }

    public enum Stars
    {
        one, two, three, four, five
    }
}
