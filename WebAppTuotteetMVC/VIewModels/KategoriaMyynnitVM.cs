using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppTuotteetMVC.ViewModels
{
    public class KategoriaMyynnitVM
    {      
        [Key]
        public long rowid { get; set; }
        public string KategoriaNimi { get; set; }
        public Nullable<decimal> KategoriaTuoteMyynnit { get; set; }
        public string Nimi { get; set; }
        public Nullable<decimal> TuoteMyynnit { get; set; }

        public Nullable<decimal> MyytyYhteensä { get; set; }
    }
}