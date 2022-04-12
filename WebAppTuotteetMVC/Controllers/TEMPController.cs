using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppTuotteetMVC.Models;
using WebAppTuotteetMVC.ViewModels;


namespace WebAppTuotteetMVC.Controllers
{
    public class TEMPController : Controller
    {

        private TilausDBEntities db = new TilausDBEntities();

        // GET: TEMP
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Kategoriamyynnit2()
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

            ViewBag.kategoriaNimiT1 = kategoriaNimiLista;
            ViewBag.kategoriaMyynnitT1 = kategoriaMyynnitLista;

            return View();
        }

        public ActionResult TopMyynnit2()
        {
            string topNimiLista;
            string topMyynnitLista;

            List<KategoriaMyynnitVM> TopTuoteMyynnit = new List<KategoriaMyynnitVM>();

            var topMyyntiData = from tm in db.KaikkiMyynnit
                                orderby tm.MyytyYhteensä descending
                                select tm;

            foreach (KaikkiMyynnit topmyynnit in topMyyntiData)
            {
                KategoriaMyynnitVM yksiMyyntiRivi = new KategoriaMyynnitVM();
                yksiMyyntiRivi.Nimi = topmyynnit.Nimi;
                yksiMyyntiRivi.MyytyYhteensä = (int?)topmyynnit.MyytyYhteensä;
                TopTuoteMyynnit.Add(yksiMyyntiRivi);
            }

            topNimiLista = "'" + string.Join("','", TopTuoteMyynnit.Select(n => n.Nimi).ToList()) + "'";
            topMyynnitLista = string.Join(",", TopTuoteMyynnit.Select(n => n.MyytyYhteensä.ToString()).ToList());

            ViewBag.topNimiT1 = topNimiLista;
            ViewBag.topMyynnitT1 = topMyynnitLista;

            return View();
        }
    }
}