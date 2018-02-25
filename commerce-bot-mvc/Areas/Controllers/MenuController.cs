using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using Bot.Dto.Entitites;
using commerce_bot_mvc.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commerce_bot_mvc.Areas.Controllers
{
    public class MenuController : ApiController
    {
        [HttpGet]
        [Route("api/menu")]
        public IHttpActionResult GetMenu([FromUri]string userId, int? restaurantId)
        {
            Dictionary<string, List<Food>> menu = new Dictionary<string, List<Food>>();
            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {
                foreach (var category in ctx.FoodCategories)
                {
                    var foodList = ctx.Food.Where(x => x.RestaurantId == restaurantId && x.FoodCategoryId == category.Id).ToList();
                    if (foodList.Count != 0)
                    {
                        menu.Add(category.FoodCategoryName, foodList);
                    }
                }
            }

            return Ok(new { data = menu });
        }

        [HttpPost]
        [Route("api/menu")]
        public IHttpActionResult ReceiveUserOrder(JObject order)
        {
            JArray nonavailability_array = (JArray)order["order"];
            List<Food> userOrder = new List<Food>();
            JsonSerializer s = new JsonSerializer();
            foreach (var item in nonavailability_array)
            {
                JObject aItem = (JObject)item;
                Food orderItem = s.Deserialize<Food>(new JsonTextReader(new StringReader(aItem.ToString())));
                userOrder.Add(orderItem);
            }

            return Ok();
        }
    }
}
