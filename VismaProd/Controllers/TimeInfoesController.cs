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
    public class TimeInfoesController : Controller
    {
        private VismaDB db = new VismaDB();

        // GET: TimeInfoes
        public ActionResult Index()
        {
            var timeInfoes = db.TimeInfoes.Include(t => t.FreeLancer).Include(t => t.Project);
            return View(timeInfoes.ToList());
        }

        // GET: TimeInfoes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeInfo timeInfo = db.TimeInfoes.Find(id);
            if (timeInfo == null)
            {
                return HttpNotFound();
            }
            return View(timeInfo);
        }

        // GET: TimeInfoes/Create
        public ActionResult Create()
        {
            ViewBag.UID = new SelectList(db.FreeLancers, "Id", "Name");
            ViewBag.PID = new SelectList(db.Projects, "Id", "Name");

            var hours = new List<int>();
            for (int i = 7; i < 22; i++)
                hours.Add(i);
            ViewBag.startHour = new SelectList(hours);
            ViewBag.endHour = new SelectList(hours);
            ViewBag.Message = "Add time to your projects!";
            return View();
        }

        // POST: TimeInfoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UID,PID,startTime,endTime")] TimeInfo timeInfo,int startHour,int endHour)
        {
            ViewBag.UID = new SelectList(db.FreeLancers, "Id", "Name", timeInfo.UID);
            ViewBag.PID = new SelectList(db.Projects, "Id", "Name", timeInfo.PID);
            var hours = new List<int>();
            for (int i = 1; i < 25; i++)
                hours.Add(i);
            ViewBag.startHour = new SelectList(hours);
            ViewBag.endHour = new SelectList(hours);
            ViewBag.Message = "Add time to your projects!";
            if (ModelState.IsValid)
            {
                
                timeInfo.startTime=timeInfo.startTime.AddHours(startHour);
                timeInfo.endTime=timeInfo.endTime.AddHours(endHour);
                if (DateTime.Compare(timeInfo.startTime, timeInfo.endTime) >= 0)
                {
                    ViewBag.Message = "Invalid Time, End Time can't be before or same as Start Time";
                    return View(timeInfo); //bad datetime
                }
                else
                {
                    timeInfo.Id = Guid.NewGuid();
                    Project proj = db.Projects.Find(timeInfo.PID);
                    int time = getHoursAdded(timeInfo);
                    timeInfo.hours = time;
                    proj.TotalHours = proj.TotalHours+time;
                    db.Projects.Attach(proj);
                    db.Entry(proj).State = EntityState.Modified;
                    db.TimeInfoes.Add(timeInfo);
                    db.SaveChanges();
                    return RedirectToAction("Index","FreeLancers");
                }
            }
            

            return View(timeInfo);
        }

        // GET: TimeInfoes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeInfo timeInfo = db.TimeInfoes.Find(id);
            if (timeInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.UID = new SelectList(db.FreeLancers, "Id", "Name", timeInfo.UID);
            ViewBag.PID = new SelectList(db.Projects, "Id", "Name", timeInfo.PID);
            return View(timeInfo);
        }

        // POST: TimeInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UID,PID,startTime,endTime")] TimeInfo timeInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timeInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UID = new SelectList(db.FreeLancers, "Id", "Name", timeInfo.UID);
            ViewBag.PID = new SelectList(db.Projects, "Id", "Name", timeInfo.PID);
            return View(timeInfo);
        }

        // GET: TimeInfoes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeInfo timeInfo = db.TimeInfoes.Find(id);

            if (timeInfo == null)
            {
                return HttpNotFound();
            }
            return View(timeInfo);
        }

        // POST: TimeInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TimeInfo timeInfo = db.TimeInfoes.Find(id);
            timeInfo.Project.TotalHours -= timeInfo.hours;
            Guid userid = new Guid();
            userid = timeInfo.FreeLancer.Id;
            db.Entry(timeInfo.Project).State = EntityState.Modified;
            db.TimeInfoes.Remove(timeInfo);
            db.SaveChanges();
            return RedirectToAction("Index", "FreeLancers");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private int getHoursAdded(TimeInfo timeInfo)
        {
            TimeSpan span = timeInfo.endTime.Subtract(timeInfo.startTime);
            int hours = (int)span.TotalHours / 24;
            hours = hours * 8;
            hours += (int)span.TotalHours % 24;
            return hours;
        }
    }


    
}
