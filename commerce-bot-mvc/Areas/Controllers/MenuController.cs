using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Bot.Dto.Entitites;
using Bot.Dto.Enums;
using commerce_bot_mvc.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commerce_bot_mvc.Areas.Controllers
{
    public class MenuController : ApiController
    {

        JsonSerializer s = new JsonSerializer();

        [HttpGet]
        [Route("api/menu")]
        public IHttpActionResult GetMenu([FromUri]string userId, int? restaurantId)
        {
            Dictionary<string, List<Food>> menu = new Dictionary<string, List<Food>>();
            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {
                var categories = ctx.FoodCategories.Distinct().ToList();
                foreach (var category in categories)
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
            JToken orderArray = order["order"];
            JArray nonavailability_array = (JArray)orderArray["order"];
            List<OrderItem> items = new List<OrderItem>();

            string userId = orderArray["userId"].ToString();
            int restaurantId = Int32.Parse(orderArray["restaurantId"].ToString());

            foreach (var item in nonavailability_array)
            {
                JObject aItem = (JObject)item;
                OrderItem orderItem = s.Deserialize<OrderItem>(new JsonTextReader(new StringReader(aItem.ToString())));
                items.Add(orderItem);
            }

            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {
                Order finalizedOrder = new Order
                {
                    OrderData = items,
                    OrderState = OrderState.NotConfirmed,
                    RestaurantId = restaurantId,
                    UserId = ctx.BotUsers.FirstOrDefault(x => x.MessengerId == userId)?.Id
                };
                ctx.Orders.Add(finalizedOrder);
                ctx.SaveChanges();
                HttpClient client = new HttpClient();
                client.GetAsync("http://demo-bot-alede.azurewebsites.net/api/proactiveMessages?userId=" + userId + "&orderId=" + finalizedOrder.Id);
            }
            return Ok();
        }
    }
}
