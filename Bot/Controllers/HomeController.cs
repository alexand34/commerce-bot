using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Bot.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index([FromBody]string userId, int? restaurantId)
        {
            ViewBag.Title = "Chat&Buy Menu";

            ViewBag.UserId = userId;
            Dictionary<string, List<Food>> menu= new Dictionary<string, List<Food>>();
            using (Entities ctx = new Entities())
            {
                ViewBag.RestaurantId = ctx.Restaurants.FirstOrDefault(x => x.Id == restaurantId);
                foreach (var category in ctx.FoodCategories)
                {
                    var foodList = category.Foods.Where(x=>x.RestaurantId == restaurantId).ToList();
                    menu.Add(category.FoodCategory1, foodList);
                }
            }

            ViewBag.Menu = menu;

            return View();
        }
    }
}
