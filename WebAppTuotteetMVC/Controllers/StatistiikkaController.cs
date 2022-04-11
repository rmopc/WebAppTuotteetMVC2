using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppTuotteetMVC.Models;
using WebAppTuotteetMVC.ViewModels;

namespace WebAppTuotteetMVC.Controllers
{
    public class StatistiikkaController : Controller
    {
        private TilausDBEntities db = new TilausDBEntities();

        // GET: Statistiikka
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Kategoriamyynnit()
        {
            string kategoriaNimiLista;
            string kategoriaMyynnitLista;

            List<KategoriaMyynnitVM> KategoriaMyynnitLista = new List<KategoriaMyynnitVM>();

            var kategoriaMyynnitData = from km in db.Kategoriamyynnit
                                                            select km;

            foreach (Kategoriamyynnit katmyynnit in kategoriaMyynnitData)
            {
                KategoriaMyynnitVM yksiMyyntiRivi = new KategoriaMyynnitVM();
                yksiMyyntiRivi.KategoriaNimi = katmyynnit.KategoriaNimi;
                yksiMyyntiRivi.KategoriaTuoteMyynnit = (int?)katmyynnit.KategoriaTuoteMyynnit;
               
                KategoriaMyynnitLista.Add(yksiMyyntiRivi);
            }

            kategoriaNimiLista = "'" + string.Join("','", KategoriaMyynnitLista.Select(n => n.KategoriaNimi).ToList()) + "'";
            kategoriaMyynnitLista = string.Join(",", KategoriaMyynnitLista.Select(n => n.KategoriaTuoteMyynnit.ToString()).ToList());

            ViewBag.kategoriaNimi = kategoriaNimiLista;
            ViewBag.kategoriaMyynnit = kategoriaMyynnitLista;

            return View();
        }

        public ActionResult TopMyynnit()
        {
            string topNimiLista;
            string topMyynnitLista;

            List<TopSales> TopTuoteMyynnit = new List<TopSales>();

            var topMyyntiData = from tm in db.KaikkiMyynnit
                                       orderby tm.MyytyYhteensä descending
                                       select tm;

            foreach (KaikkiMyynnit topmyynnit in topMyyntiData)
            {
                TopSales yksiMyyntiRivi = new TopSales();
                yksiMyyntiRivi.Nimi = topmyynnit.Nimi;
                yksiMyyntiRivi.MyytyYhteensä = (int?)topmyynnit.MyytyYhteensä;
                TopTuoteMyynnit.Add(yksiMyyntiRivi);
            }

            topNimiLista = "'" + string.Join("','", TopTuoteMyynnit.Select(n => n.Nimi).ToList()) + "'";
            topMyynnitLista = string.Join(",", TopTuoteMyynnit.Select(n => n.MyytyYhteensä.ToString()).ToList());

            ViewBag.topNimi = topNimiLista;
            ViewBag.topMyynnit = topMyynnitLista;

            return View();
        }
    }
}