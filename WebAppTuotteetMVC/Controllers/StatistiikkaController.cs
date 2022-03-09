﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppTuotteetMVC.Models;
using WebAppTuotteetMVC.VIewModels;

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
    }
}