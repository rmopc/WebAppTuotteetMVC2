using System;


namespace WebAppTuotteetMVC.VIewModels
{
    public class TilausYhteenveto
    {
        public int TuoteID { get; set; }
        public string Nimi { get; set; }
        public Nullable<decimal> Ahinta { get; set; }
        public byte[] Kuva { get; set; }
        public string ImageLink { get; set; }        
        public int TilausriviID { get; set; }         
        public Nullable<int> Maara { get; set; }       
        public int TilausID { get; set; }
        public Nullable<int> AsiakasID { get; set; }
        public string Toimitusosoite { get; set; }
        public string Postinumero { get; set; }
        public Nullable<System.DateTime> Tilauspvm { get; set; }
        public Nullable<System.DateTime> Toimituspvm { get; set; }
        public int KategoriaID { get; set; }
        public string KategoriaNimi { get; set; }
        public string KategoriaIDKategoriaNimi { get; set; }
    }
}