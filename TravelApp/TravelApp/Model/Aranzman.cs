using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Model
{
    public class Aranzman : IEquatable<Aranzman>
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
            Smestaji = smestaji;
            Atrakcije = atrakcije;
        }


        public Aranzman(int id, string name, string description, DateTime startDate, DateTime endDate, string startLocation, string endLocation, double price, string pictureLocation)
        {
            Id = id;
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            StartLocation = startLocation;
            EndLocation = endLocation;
            Price = price;
            PictureLocation = pictureLocation;
            Restorani = new List<Restoran>();
            Atrakcije = new List<Atrakcija>();
            Smestaji = new List<Smestaj>();
        }

        public Aranzman(string name, string description, DateTime startDate, DateTime endDate, string startLocation, string endLocation, double price, string pictureLocation)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            StartLocation = startLocation;
            EndLocation = endLocation;
            Price = price;
            PictureLocation = pictureLocation;
            Restorani = new List<Restoran>();
            Atrakcije = new List<Atrakcija>();
            Smestaji = new List<Smestaj>();
        }

        public bool Equals(Aranzman other)
        {
            if (other == null)
                return false;

            return
                this.Id != null &&
                this.Id.Equals(other.Id);
        }

        override public string ToString() 
        {
            return this.Id.ToString() + " " + this.Name;
        }
    }
}
