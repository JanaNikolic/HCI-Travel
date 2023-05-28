using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Model
{
    class Aranzman
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string startLocation { get; set; }
        public string endLocation { get; set; }
        public double Price { get; set; }

        public int RestoranId { get; set; }
        public virtual Restoran Restoran { get; set; }

        public int AtrakcijaId { get; set; }
        public virtual Atrakcija Atrakcija { get; set; }

        public int SmestajId { get; set; }
        public virtual Smestaj Smestaj { get; set; }
    }
}
