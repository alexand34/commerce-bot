//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Http;
//using System.Web.Mvc;
//using Bot.Dto.Entitites;
//using commerce_bot_mvc.Models;
//using System.Web.Helpers;
//using Bot.Dto.Dtos;
//using Newtonsoft.Json;

//namespace commerce_bot_mvc.Controllers
//{
//    public class MenuController : Controller
//    {
//        // GET: Menu
//        public ActionResult Index([FromBody]string userId, int? restaurantId)
//        {
//            ViewBag.Title = "Chat&Buy Menu";

//            Dictionary<string, List<Food>> menu = new Dictionary<string, List<Food>>();
//            using (ApplicationDbContext ctx = new ApplicationDbContext())
//            {

//                var restaurant = ctx.Restaurants.FirstOrDefault(x => x.Id == restaurantId);
//                ViewBag.RestaurantId = restaurant?.RestaurantName;
//                foreach (var category in ctx.FoodCategories)
//                {
//                    var foodList = category.Foods.Where(x => x.RestaurantId == restaurantId).ToList();
//                    if (foodList.Count != 0)
//                    {
//                        menu.Add(category.FoodCategoryName, foodList);
//                    }
//                }
//            }

//            ViewBag.Menu = menu;
//            List<Menu> jsonMenu = new List<Menu>();
//            foreach (var item in menu)
//            {
//                jsonMenu.Add(new Menu(){
//                    Category = item.Key,
//                    Foods = item.Value
//                });
//            }
//            ViewBag.JsonMenu = JsonConvert.SerializeObject(jsonMenu.ToString());
//            return View();
//        }
//    }
//}