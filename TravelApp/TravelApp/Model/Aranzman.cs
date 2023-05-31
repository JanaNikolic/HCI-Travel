using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Model
{
    public class Aranzman
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public double Price { get; set; }

        public string PictureLocation { get; set; }

        public List<Restoran> Restorani { get; set; }
        public List<Atrakcija> Atrakcije { get; set; }
        public List<Smestaj> Smestaji { get; set; }

        public Aranzman() {
            Restorani = new List<Restoran>();
            Atrakcije = new List<Atrakcija>();
            Smestaji = new List<Smestaj>();
        }

        public Aranzman(string name, string description, DateTime startDate, DateTime endDate, string startLocation, string endLocation, double price, string pictureLocation, List<Restoran> restorani, List<Atrakcija> atrakcije, List<Smestaj> smestaji)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            StartLocation = startLocation;
            EndLocation = endLocation;
            Price = price;
            PictureLocation = pictureLocation;
            Restorani = restorani;
            Atrakcije = atrakcije;
            Smestaji = smestaji;
        }
    }
}
