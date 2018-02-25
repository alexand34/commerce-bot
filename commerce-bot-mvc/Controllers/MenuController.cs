using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Bot.Dto.Entitites;
using commerce_bot_mvc.Models;

namespace commerce_bot_mvc.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        public ActionResult Index([FromBody]string userId, int? restaurantId)
        {
            ViewBag.Title = "Chat&Buy Menu";

            Dictionary<string, List<Food>> menu = new Dictionary<string, List<Food>>();
            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {

                var restaurant = ctx.Restaurants.FirstOrDefault(x => x.Id == restaurantId);
                ViewBag.RestaurantId = restaurant?.RestaurantName;
                foreach (var category in ctx.FoodCategories)
                {
                    var foodList = category.Foods.Where(x => x.RestaurantId == restaurantId).ToList();
                    if (foodList.Count != 0)
                    {
                        menu.Add(category.FoodCategoryName, foodList);
                    }
                }
            }

            ViewBag.Menu = menu;

            return View();
        }
    }
}