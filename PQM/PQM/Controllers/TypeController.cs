using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PQM.Models;

namespace PQM.Controllers
{
    public class TypeController : Controller
    {
        private project_infoEntities db = new project_infoEntities();

        // GET: /Type/
        public ActionResult Index()
        {
            return View(db.type_project.ToList());
        }

        // GET: /Type/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            type_project type_project = db.type_project.Find(id);
            if (type_project == null)
            {
                return HttpNotFound();
            }
            return View(type_project);
        }

        // GET: /Type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,name,country")] type_project type_project)
        {
            if (ModelState.IsValid)
            {
                db.type_project.Add(type_project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(type_project);
        }

        // GET: /Type/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            type_project type_project = db.type_project.Find(id);
            if (type_project == null)
            {
                return HttpNotFound();
            }
            return View(type_project);
        }

        // POST: /Type/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,name,country")] type_project type_project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(type_project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(type_project);
        }

        // GET: /Type/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            type_project type_project = db.type_project.Find(id);
            if (type_project == null)
            {
                return HttpNotFound();
            }
            return View(type_project);
        }

        // POST: /Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            type_project type_project = db.type_project.Find(id);
            db.type_project.Remove(type_project);
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
