using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Autofac;
using Bot.Dto.Entitites;
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
            message.Text = "Here is your order:\n\n";
            var orderItems = ctx.OrderItems.Where(x => x.OrderId == order).ToList<OrderItem>();
            double totalPrice = 0.0;
            foreach (var item in orderItems)
            {
                Food food = ctx.Food.FirstOrDefault(x => x.Id == item.FoodId);
                message.Text += $"• {food.DishName} (x{item.Count}) – C${food.Price * item.Count}\n\n";
                totalPrice += food.Price * item.Count;
            }

            message.Text += $"Price of order: C${totalPrice}\n\n";
            message.Text += $"Price of delivery: not implemented yet\n\n";
            message.Text += $"Total:  C${totalPrice} + price for delivery";
            message.Locale = "en-us";
            message.AttachmentLayout = AttachmentLayoutTypes.List;
            message.Attachments = new List<Attachment>();
            message.Attachments.Add(GetConfirmButtonsCard());
            await connector.Conversations.SendToConversationAsync((Activity)message);

            return Ok();
        }

        private Attachment GetConfirmButtonsCard()
        {
            var buttons = CreateButtons();
            HeroCard langCard = new HeroCard()
            {
                Buttons = buttons,
            };

            return langCard.ToAttachment();
        }

        private List<CardAction> CreateButtons()
        {
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction ConfirmButton = new CardAction()
            {
                Type = "imBack",
                Title = "Confirm",
                Value = "Confirm"
            };
            CardAction CancelButton = new CardAction()
            {
                Type = "imBack",
                Title = "Cancel",
                Value = "Cancel"
            };
            cardButtons.Add(ConfirmButton);
            cardButtons.Add(CancelButton);

            return cardButtons;
        }
    }
}
