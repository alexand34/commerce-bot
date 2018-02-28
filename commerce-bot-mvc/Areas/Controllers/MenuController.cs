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
            List<OrderItem> userOrder = new List<OrderItem>();
            double price = 0.0;

            JToken orderArray = order["order"];
            JArray nonavailability_array = (JArray)orderArray["order"];

            string userId = orderArray["userId"].ToString();
            int restaurantId = Int32.Parse(orderArray["restaurantId"].ToString());

            foreach (var item in nonavailability_array)
            {
                JObject aItem = (JObject)item;
                Food foodItem = s.Deserialize<Food>(new JsonTextReader(new StringReader(aItem.ToString())));
                var orderItem = new OrderItem()
                {
                    Count = 1,
                    FoodId = foodItem.Id
                };
                price += (orderItem.Food.Price * orderItem.Count);
                userOrder.Add(orderItem);
            }

            int finalizedOrderId;
            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {
                Order finalizedOrder = new Order
                {
                    OrderData = userOrder,
                    OrderState = OrderState.NotConfirmed,
                    User = ctx.BotUsers.FirstOrDefault(x => x.MessengerId == userId),
                    Restaurant = ctx.Restaurants.FirstOrDefault(x => x.Id == restaurantId),
                    Price = price
                };

                ctx.Orders.Add(finalizedOrder);
                ctx.SaveChanges();
                finalizedOrderId = finalizedOrder.Id;
            }


            HttpClient client = new HttpClient();
            client.GetAsync("http://localhost:8039/api/proactiveMessages?userId=" + userId + "&orderId=" + finalizedOrderId);

            return Ok();
        }
    }
}
