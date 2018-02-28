using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using commerce_bot_mvc.Models;
using Microsoft.Bot.Connector;

namespace commerce_bot_mvc.Areas.Controllers
{
    public class ProactiveMessagesController : ApiController
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();
        [HttpGet]
        [Route("api/ProactiveMessages")]
        public async Task<OkResult> SendProactiveMessages([FromUri] string userId, string orderId)
        {
            ApplicationDbContext ctx = new ApplicationDbContext();

            int order = Int32.Parse(orderId);
            var recepient = ctx.BotUsers.FirstOrDefault(x => x.MessengerId == userId);
            var finalizedOrder = ctx.Orders.FirstOrDefault(x => x.Id == order);
            // Use the data stored previously to create the required objects.
            var userAccount = new ChannelAccount(recepient.toId, recepient.toName);
            var botAccount = new ChannelAccount(recepient.fromId, recepient.fromName);
            var connector = new ConnectorClient(new Uri(recepient.serviceUrl));
            // Create a new message.
            IMessageActivity message = Activity.CreateMessageActivity();
            if (!string.IsNullOrEmpty(recepient.conversationId) && !string.IsNullOrEmpty(recepient.channelId))
            {
                // If conversation ID and channel ID was stored previously, use it.
                message.ChannelId = recepient.channelId;
            }
            else
            {
                // Conversation ID was not stored previously, so create a conversation. 
                // Note: If the user has an existing conversation in a channel, this will likely create a new conversation window.
                recepient.conversationId = (await connector.Conversations.CreateDirectConversationAsync(botAccount, userAccount))
                    .Id;
            }

            // Set the address-related properties in the message and send the message.
            message.From = botAccount;
            message.Recipient = userAccount;
            message.Conversation = new ConversationAccount(id: recepient.conversationId);
            message.Text = "Here is your order:\n";
            foreach (var item in finalizedOrder.OrderData)
            {
                message.Text += $"• {item.Food.DishName} (x{item.Count}) – C${item.Food.Price * item.Count}\n";
            }

            message.Text += $"Price of order: C${finalizedOrder.Price}\n";
            message.Text += $"Price of delivery: not implemented yet\n";
            message.Text += $"Total:  C${finalizedOrder.Price} + price for delivery";
            message.Locale = "en-us";
            await connector.Conversations.SendToConversationAsync((Activity) message);

            return Ok();
        }
    }
}
