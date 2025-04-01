using GMap.NET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CografiBilgiSistemi1
{
    internal class Arac
    {
        private string plaka;
        private string tipi;
        private string baslangic;
        private string bitis;
        private PointLatLng konum;


    

    public Arac(string plaka, string tipi, string baslangic, string bitis, PointLatLng konum)
        {
            this.Plaka = plaka;
            this.Tipi = tipi;
            this.Baslangic = baslangic;
            this.Bitis = bitis;
            this.Konum = konum;
        }

        public string Plaka { get => plaka; set => plaka = value; }
        public string Tipi { get => tipi; set => tipi = value; }
        public string Baslangic { get => baslangic; set => baslangic = value; }
        public string Bitis { get => bitis; set => bitis = value; }
        public PointLatLng Konum { get => konum; set => konum = value; }

        public override string ToString()
        {
            string str = "\n" + Plaka + "\n" + Tipi + "\n" + Baslangic + "\n" + Bitis;
            return str;
        }
    }

}