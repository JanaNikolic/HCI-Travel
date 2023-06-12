using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Model
{
    public class MesecniIzvestaj
    {
        public string Mesec { get; set; }
        public string NazivAranzmana { get; set; }
        public int BrProdatih { get; set; }
        public double Cena { get; set; }
        public double UkupnaCena { get; set; }

        public MesecniIzvestaj() { }

        public MesecniIzvestaj(string mesec, string nazivAranzmana, int brProdatih, double cena, double ukupnaCena)
        {
            Mesec = mesec;
            NazivAranzmana = nazivAranzmana;
            BrProdatih = brProdatih;
            Cena = cena;
            UkupnaCena = ukupnaCena;
        }
    }
}
