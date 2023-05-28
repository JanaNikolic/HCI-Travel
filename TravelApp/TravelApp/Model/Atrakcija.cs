using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TravelApp.Model
{
    class Atrakcija
    {
        [Key] public int Id { get; set; }

        [Column(TypeName = "varbinary(max)")]
        public byte[] Picture { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
    }
}
