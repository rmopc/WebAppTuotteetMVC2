using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppTuotteetMVC.ViewModels
{
    public class TopSales
    {
        public string Nimi { get; set; }
        public Nullable<decimal>  MyytyYhteensä { get; set; }
    }
}