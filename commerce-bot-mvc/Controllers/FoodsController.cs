using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bot.Dto.Entitites;
using commerce_bot_mvc.Models;

namespace commerce_bot_mvc.Controllers
{
    public class FoodsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Foods
        public ActionResult Index()
        {
            var food = db.Food.Include(f => f.FoodCategory).Include(f => f.Restaurant);
            return View(food.ToList());
        }

        // GET: Foods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Food.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // GET: Foods/Create
        public ActionResult Create()
        {
            ViewBag.FoodCategoryId = new SelectList(db.FoodCategories, "Id", "FoodCategoryName");
            ViewBag.RestaurantId = new SelectList(db.Restaurants, "Id", "RestaurantName");
            return View();
        }

        // POST: Foods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FoodCategoryId,RestaurantId,DishName,Price,Portion,DishDescription")] Food food)
        {
            if (ModelState.IsValid)
            {
                db.Food.Add(food);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FoodCategoryId = new SelectList(db.FoodCategories, "Id", "FoodCategoryName", food.FoodCategoryId);
            ViewBag.RestaurantId = new SelectList(db.Restaurants, "Id", "RestaurantName", food.RestaurantId);
            return View(food);
        }

        // GET: Foods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Food.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            ViewBag.FoodCategoryId = new SelectList(db.FoodCategories, "Id", "FoodCategoryName", food.FoodCategoryId);
            ViewBag.RestaurantId = new SelectList(db.Restaurants, "Id", "RestaurantName", food.RestaurantId);
            return View(food);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FoodCategoryId,RestaurantId,DishName,Price,Portion,DishDescription")] Food food)
        {
            if (ModelState.IsValid)
            {
                db.Entry(food).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FoodCategoryId = new SelectList(db.FoodCategories, "Id", "FoodCategoryName", food.FoodCategoryId);
            ViewBag.RestaurantId = new SelectList(db.Restaurants, "Id", "RestaurantName", food.RestaurantId);
            return View(food);
        }

        // GET: Foods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Food.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // POST: Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Food food = db.Food.Find(id);
            db.Food.Remove(food);
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
