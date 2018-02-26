using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Connector;

namespace commerce_bot_mvc.Models
{
    public class ConversationStarter
    {
        //Note: Of course you don't want these here. Eventually you will need to save these in some table
        //Having them here as static variables means we can only remember one user :)
        public static string fromId;
        public static string fromName;
        public string toId;
        public string toName;
        public string serviceUrl;
        public string channelId;
        public string conversationId;

        //This will send an adhoc message to the user
        public async Task Resume(string conversationId, string channelId)
        {
            var userAccount = new ChannelAccount(toId, toName);
            var botAccount = new ChannelAccount(fromId, fromName);
            var connector = new ConnectorClient(new Uri(serviceUrl));

            IMessageActivity message = Activity.CreateMessageActivity();
            if (!string.IsNullOrEmpty(conversationId) && !string.IsNullOrEmpty(channelId))
            {
                message.ChannelId = channelId;
            }
            else
            {
                conversationId = (await connector.Conversations.CreateDirectConversationAsync(botAccount, userAccount)).Id;
            }
            message.From = botAccount;
            message.Recipient = userAccount;
            message.Conversation = new ConversationAccount(id: conversationId);
            message.Text = "Hello, this is a notification";
            message.Locale = "en-Us";
            await connector.Conversations.SendToConversationAsync((Activity)message);
        }
    }
}