﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppTuotteetMVC.Models;
using WebAppTuotteetMVC.ViewModels;

namespace WebAppTuotteetMVC.Controllers
{
    public class TilauksetController : Controller
    {
        private TilausDBEntities db = new TilausDBEntities();

        // GET: Tilaukset
        public ActionResult Index()
        {
            var tilaukset = db.Tilaukset.Include(t => t.Asiakkaat).Include(t => t.Postitoimipaikat);
            ViewBag.RiviLkm = db.Tilaukset.Count();
            return View(tilaukset.ToList());
        }

        // GET: Tilaukset/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            if (tilaukset == null)
            {
                return HttpNotFound();
            }
            return View(tilaukset);
        }

        // GET: Tilaukset/Create
        public ActionResult Create()
        {
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi");
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka");
            return View();
        }

        // POST: Tilaukset/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TilausID,AsiakasID,Toimitusosoite,Postinumero,Tilauspvm,Toimituspvm")] Tilaukset tilaukset)
        {
            if (ModelState.IsValid)
            {
                db.Tilaukset.Add(tilaukset);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi", tilaukset.AsiakasID);
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka", tilaukset.Postinumero);
            
            return View(tilaukset);
        }

        // GET: Tilaukset/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            if (tilaukset == null)
            {
                return HttpNotFound();
            }
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi", tilaukset.AsiakasID);
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka", tilaukset.Postinumero);
            return View(tilaukset);
        }

        // POST: Tilaukset/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TilausID,AsiakasID,Toimitusosoite,Postinumero,Tilauspvm,Toimituspvm")] Tilaukset tilaukset)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tilaukset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi", tilaukset.AsiakasID);
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka", tilaukset.Postinumero);
            return View(tilaukset);
        }

        // GET: Tilaukset/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            if (tilaukset == null)
            {
                return HttpNotFound();
            }
            return View(tilaukset);
        }

        // POST: Tilaukset/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            db.Tilaukset.Remove(tilaukset);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult TilausYhteenveto()
        {
            var tilausYhteenveto = from t in db.Tilaukset
                                   join tr in db.Tilausrivit on t.TilausID equals tr.TilausID
                                   join tu in db.Tuotteet on tr.TuoteID equals tu.TuoteID
                                   join k in db.Kategoriat on tu.KategoriaID equals k.KategoriaID

                                   select new TilausYhteenveto
                                   {
                                       TuoteID = tu.TuoteID,
                                       Nimi = tu.Nimi,
                                       Ahinta = (decimal)tu.Ahinta,
                                       //TilausriviID = (int)tr.TilausID,
                                       Maara = (int)tr.Maara,
                                       TilausID = (int)t.TilausID,
                                       AsiakasID = (int)t.AsiakasID,
                                       Toimitusosoite = t.Toimitusosoite,
                                       Postinumero = t.Postinumero,
                                       Tilauspvm = (DateTime)t.Tilauspvm,
                                       Toimituspvm = (DateTime)t.Toimituspvm,
                                       KategoriaID = (int)k.KategoriaID,
                                       KategoriaNimi = k.KategoriaNimi,
                                   };
            ViewBag.RiviLkm = db.Tilaukset.Count();
            return View(tilausYhteenveto);
        }

        public ActionResult TilausOtsikot()
        {
            var tilaukset = db.Tilaukset.Include(t => t.Asiakkaat).Include(t => t.Postitoimipaikat);            
            return View(tilaukset.ToList());
        }

        public ActionResult _TilausRivit(int? orderid)
        {
            var orderRowsList = from tr in db.Tilausrivit                                   
                                   join tu in db.Tuotteet on tr.TuoteID equals tu.TuoteID
                                   join k in db.Kategoriat on tu.KategoriaID equals k.KategoriaID
                                   where tr.TilausID == orderid

                                   select new OrderRows
                                   {
                                       TilausID =tr.TilausID, //tähän piti lisätä tuo int suluissa
                                       Nimi = tu.Nimi,
                                       Ahinta = (decimal)tu.Ahinta,                                       
                                       Maara = (int)tr.Maara,
                                       KategoriaID = (int)k.KategoriaID,
                                       KategoriaNimi = k.KategoriaNimi,
                                       TuoteID = (int)tu.TuoteID
                                   };
            //ViewBag.RiviLkm = db.Tilaukset.Count();
            return PartialView(orderRowsList);
        }

        public ActionResult _ModalEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            if (tilaukset == null)
            {
                return HttpNotFound();
            }
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi", tilaukset.AsiakasID);
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka", tilaukset.Postinumero);
            //ViewBag.ShipVia = new SelectList(db.Tilausrivit, "TilausID", "TuoteID", orders.Tilausrivit);
            return PartialView("_ModalEdit", tilaukset); //palautetaan PartialView ja sille parametrinä yhden tilauksen tiedot
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _ModalEdit([Bind(Include = "TilausID,AsiakasID,Toimitusosoite,Postinumero,Tilauspvm,Toimituspvm")] Tilaukset tilaukset)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tilaukset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi", tilaukset.AsiakasID);
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka", tilaukset.Postinumero);
            //ViewBag.ShipVia = new SelectList(db.Tilausrivit, "TilausID", "TuoteID", orders.Tilausrivit);
            return PartialView("_ModalEdit", tilaukset);
        }

        public ActionResult _ModalDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            if (tilaukset == null)
            {
                return HttpNotFound();
            }
            return PartialView("_ModalDelete", tilaukset);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("_ModalDelete")]
        [ValidateAntiForgeryToken]

        public ActionResult _ModalDeleteConfirmed(int id)
        {
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            db.Tilaukset.Remove(tilaukset);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult DeleteFromJQuery(string id)
        {
            TilausDBEntities db = new TilausDBEntities();
            int iid = int.Parse(id);
            // etsitään id:n perusteella asiakasrivi kannasta
            bool OK = false;
            Tilaukset dbItem = (from t in db.Tilaukset
                                                where t.TilausID == iid
                                                select t).FirstOrDefault();
            if (dbItem != null)
            {
                // tietokannasta poisto
                db.Tilaukset.Remove(dbItem);
                db.SaveChanges();
                OK = true;
            }
            db.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);
        }


        public ActionResult _ModalCreate()
        {
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi"/*, tilaukset.AsiakasID*/);
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka"/*, tilaukset.Postinumero*/);
            return PartialView();
        }
    }
}
