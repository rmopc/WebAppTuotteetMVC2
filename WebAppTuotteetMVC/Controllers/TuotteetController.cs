using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppTuotteetMVC.Models;
using PagedList;

namespace WebAppTuotteetMVC.Controllers
{
    public class TuotteetController : Controller
    {
        private TilausDBEntities db = new TilausDBEntities();

        // GET: Tuotteet
        public ActionResult Index()
        {
            ViewBag.RiviLkm = db.Tuotteet.Count();

            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Logged out";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewBag.LoggedStatus = Session["UserName"].ToString();
                return View(db.Tuotteet.ToList());
            }            
        }

        public ActionResult Index2(string searchString1, string sortOrder, string currentFilter1, int? page, int? pagesize, string TuoteKategoria, string currentProductCategory)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.ProductNameSortParm = String.IsNullOrEmpty(sortOrder) ? "productname_desc" : "";
            ViewBag.UnitPriceSortParm = sortOrder == "UnitPrice" ? "UnitPrice_desc" : "UnitPrice";

            if (searchString1 != null)
            {
                page = 1;
            }
            else
            {
                searchString1 = currentFilter1;
            }

            ViewBag.currentFilter = searchString1;

            if ((TuoteKategoria!= null) && (TuoteKategoria != "0"))
            {
                page = 1;
            }
            else
            {
                TuoteKategoria = currentProductCategory;
            }

            ViewBag.currentProductCategory = TuoteKategoria;

            var tuotteet = from t in db.Tuotteet.Include(t => t.Kategoriat) //tällä saadaan näkymiin kategoria tuote-index2 näkymässä
                           select t;
            if(!String.IsNullOrEmpty(TuoteKategoria) && (TuoteKategoria != "0"))
            {
                int para = int.Parse(TuoteKategoria);
                tuotteet = tuotteet.Where(t =>t.KategoriaID == para);
            }


            if (!String.IsNullOrEmpty(searchString1))
            {
                switch (sortOrder)
                {
                    case "productname_desc":
                        tuotteet = tuotteet.Where(t => t.Nimi.Contains(searchString1)).OrderByDescending(t => t.Nimi);
                        break;
                    case "UnitPrice":
                        tuotteet = tuotteet.Where(t => t.Nimi.Contains(searchString1)).OrderBy(t => t.Ahinta);
                        break;
                    case "UnitPrice_desc":
                        tuotteet = tuotteet.Where(t => t.Nimi.Contains(searchString1)).OrderByDescending(t => t.Ahinta);
                        break;
                    default:
                        tuotteet = tuotteet.Where(t => t.Nimi.Contains(searchString1)).OrderBy(t => t.Nimi);
                        break;
                }
            }

            else if (!String.IsNullOrEmpty(TuoteKategoria) && (TuoteKategoria != "0"))
            {
                int para = int.Parse(TuoteKategoria);
                switch (sortOrder)
                {
                    case "productname_desc":
                        tuotteet = tuotteet.Where(t => t.KategoriaID ==para).OrderByDescending(t => t.Nimi);
                        break;
                    case "UnitPrice":
                        tuotteet = tuotteet.Where(t => t.KategoriaID == para).OrderBy(t => t.Ahinta);
                        break;
                    case "UnitPrice_desc":
                        tuotteet = tuotteet.Where(t => t.KategoriaID == para).OrderByDescending(t => t.Ahinta);
                        break;
                    default:
                        tuotteet = tuotteet.Where(t => t.KategoriaID == para).OrderBy(t => t.Nimi);
                        break;
                }
            }

            else //tämä ei ole käytössä koska paginointi
            {
                switch (sortOrder)
                {
                    case "productname_desc":
                        tuotteet = tuotteet.OrderByDescending(t => t.Nimi);
                        break;
                    case "UnitPrice":
                        tuotteet = tuotteet.OrderBy(t => t.Ahinta);
                        break;
                    case "UnitPrice_desc":
                        tuotteet = tuotteet.OrderByDescending(t => t.Ahinta);
                        break;
                    default:
                        tuotteet = tuotteet.OrderBy(t => t.Nimi);
                        break;
                }
            }
            List <Kategoriat> katLista = new List<Kategoriat>();

            var kategoriaLista = from kat in db.Kategoriat
                           select kat;

            Kategoriat tyhjaKategoria = new Kategoriat();
            tyhjaKategoria.KategoriaID = 0;
            tyhjaKategoria.KategoriaNimi = "";
            tyhjaKategoria.KategoriaIDKategoriaNimi = "";
            katLista.Add(tyhjaKategoria);

            foreach (Kategoriat kategoria in kategoriaLista)
            {
                Kategoriat yksiKategoria = new Kategoriat();
                yksiKategoria.KategoriaID = kategoria.KategoriaID;
                yksiKategoria.KategoriaNimi = kategoria.KategoriaNimi;
                yksiKategoria.KategoriaIDKategoriaNimi = kategoria.KategoriaID.ToString() + " - " + kategoria.KategoriaNimi;
                katLista.Add(yksiKategoria);
            }

            ViewBag.KategoriaID = new SelectList(katLista, "KategoriaID", "KategoriaIDKategoriaNimi", TuoteKategoria);
            int pageSize = (pagesize ?? 10);
            int pageNumber = (page ?? 1);
            return View(tuotteet.ToPagedList(pageNumber, pageSize));
        }

            // GET: Tuotteet/Details/5
            public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tuotteet tuotteet = db.Tuotteet.Find(id);
            if (tuotteet == null)
            {
                return HttpNotFound();
            }
            return View(tuotteet);
        }

        // GET: Tuotteet/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tuotteet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TuoteID,Nimi,Ahinta,Kuva")] Tuotteet tuotteet)
        {
            if (ModelState.IsValid)
            {
                db.Tuotteet.Add(tuotteet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tuotteet);
        }

        // GET: Tuotteet/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tuotteet tuotteet = db.Tuotteet.Find(id);
            if (tuotteet == null)
            {
                return HttpNotFound();
            }
            return View(tuotteet);
        }

        // POST: Tuotteet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TuoteID,Nimi,Ahinta,Kuva")] Tuotteet tuotteet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tuotteet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tuotteet);
        }

        // GET: Tuotteet/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tuotteet tuotteet = db.Tuotteet.Find(id);
            if (tuotteet == null)
            {
                return HttpNotFound();
            }
            return View(tuotteet);
        }

        // POST: Tuotteet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tuotteet tuotteet = db.Tuotteet.Find(id);
            db.Tuotteet.Remove(tuotteet);
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
    }
}
