using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebAppTuotteetMVC.ViewModels
{
    public class DailyProductSales
    {
        public string Tilauspvm { get; set; }
        public string Nimi { get; set; }

        public float DailySales { get; set; }
    }
}