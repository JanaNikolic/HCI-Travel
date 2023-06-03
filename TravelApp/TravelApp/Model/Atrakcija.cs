using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TravelApp.Model
{
    public class Atrakcija
    {
        [Key] public int Id { get; set; }

        [AllowNull]
        public string PictureLocation { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }

        public Atrakcija(string Name, string Description, string Address)
        {
            this.Name = Name;
            this.Description = Description;
            this.Address = Address;
        }

        public Atrakcija() { }
    }
}
