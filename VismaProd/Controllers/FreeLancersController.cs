using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VismaProd;

namespace VismaProd.Controllers
{
    public class FreeLancersController : Controller
    {
        private VismaDB db = new VismaDB();

        // GET: FreeLancers
        public ActionResult Index()
        {
            return View(db.FreeLancers.ToList());
        }

        // GET: FreeLancers/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FreeLancer freeLancer = db.FreeLancers.Find(id);           
            if (freeLancer == null)
            {
                return HttpNotFound();
            }

            ViewBag.UserName = freeLancer.Name;
            return View(freeLancer.TimeInfoes.ToList());
        }

        // GET: FreeLancers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FreeLancers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] FreeLancer freeLancer)
        {
            if (ModelState.IsValid)
            {
                freeLancer.Id = Guid.NewGuid();
                db.FreeLancers.Add(freeLancer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(freeLancer);
        }

        // GET: FreeLancers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FreeLancer freeLancer = db.FreeLancers.Find(id);
            if (freeLancer == null)
            {
                return HttpNotFound();
            }
            return View(freeLancer);
        }

        // POST: FreeLancers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] FreeLancer freeLancer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(freeLancer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(freeLancer);
        }

        // GET: FreeLancers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FreeLancer freeLancer = db.FreeLancers.Find(id);
            if (freeLancer == null)
            {
                return HttpNotFound();
            }
            return View(freeLancer);
        }

        // POST: FreeLancers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            FreeLancer freeLancer = db.FreeLancers.Find(id);
            db.FreeLancers.Remove(freeLancer);
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
