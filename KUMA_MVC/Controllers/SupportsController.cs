using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KUMA_MVC.Helpers;
using KUMA_MVC.Models;

namespace KUMA_MVC.Controllers
{
    public class SupportsController : Controller
    {
        private KumaModel db = new KumaModel();
        private Helper hp = new Helper();

        // GET: Supports
        public ActionResult Index()
        {
            List<Support> supports = db.Supports.OrderBy(x => x.SupportID).ToList();
            return View("../BackSystem/ListAllSupports", supports);
        }

        // GET: Supports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Support support = db.Supports.Find(id);
            if (support == null)
            {
                return HttpNotFound();
            }
            return View(support);
        }

        // GET: Supports/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.StatusID = new SelectList(db.Status, "StasusID", "StatusName");
            ViewBag.SupportCategoryID = new SelectList(db.SupportCategories, "SupportCategoryID", "SupportCategoryName");
            return View();
        }

        // POST: Supports/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SupportID,UserID,SupportCategoryID,SupportTitle,SupportContent,StatusID,SupportTime")] Support support)
        {
            support.SupportTime = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Supports.Add(support);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", support.UserID);
            ViewBag.StatusID = new SelectList(db.Status, "StasusID", "StatusName", support.StatusID);
            ViewBag.SupportCategoryID = new SelectList(db.SupportCategories, "SupportCategoryID", "SupportCategoryName", support.SupportCategoryID);
            return View(support);
        }

        // GET: Supports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Support support = db.Supports.Find(id);
            if (support == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", support.UserID);
            ViewBag.StatusID = new SelectList(db.Status, "StasusID", "StatusName", support.StatusID);
            ViewBag.SupportCategoryID = new SelectList(db.SupportCategories, "SupportCategoryID", "SupportCategoryName", support.SupportCategoryID);
            return View(support);
        }

        // POST: Supports/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SupportID,UserID,SupportCategoryID,SupportTitle,SupportContent,StatusID,SupportTime")] Support support)
        {
            if (ModelState.IsValid)
            {
                db.Entry(support).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", support.UserID);
            ViewBag.StatusID = new SelectList(db.Status, "StasusID", "StatusName", support.StatusID);
            ViewBag.SupportCategoryID = new SelectList(db.SupportCategories, "SupportCategoryID", "SupportCategoryName", support.SupportCategoryID);
            return View(support);
        }

        // GET: Supports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Support support = db.Supports.Find(id);
            if (support == null)
            {
                return HttpNotFound();
            }
            return View(support);
        }

        // POST: Supports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Support support = db.Supports.Find(id);
            db.Supports.Remove(support);
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
