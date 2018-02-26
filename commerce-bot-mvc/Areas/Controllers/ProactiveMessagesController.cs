using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using commerce_bot_mvc.Models;

namespace commerce_bot_mvc.Areas.Controllers
{
    public class ProactiveMessagesController : ApiController
    {
        private async Task<HttpResponseMessage> SendProactiveMessages(string fromId)
        {
            ConversationStarter conversationStarter = new ConversationStarter();
            try
            {
                if (!string.IsNullOrEmpty(fromId))
                {
                    await conversationStarter.Resume(conversationStarter.conversationId, conversationStarter.channelId); //We don't need to wait for this, just want to start the interruption here

                    var resp = new HttpResponseMessage(HttpStatusCode.OK);
                    resp.Content = new StringContent($"<html><body>Message sent, thanks.</body></html>", System.Text.Encoding.UTF8, @"text/html");
                    return resp;
                }
                else
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.OK);
                    resp.Content = new StringContent($"<html><body>You need to talk to the bot first so it can capture your details.</body></html>", System.Text.Encoding.UTF8, @"text/html");
                    return resp;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
