using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.Dialogs
{
    [Serializable]
    public class LocationDialog : IDialog<string>
    {
        private string Category;
        private int attempts = 3;

        public LocationDialog(string category)
        {
            this.Category = category;
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"Okay, category is set. Please, provide you location for us to choose the best " +
                                    $"{ Category } food in your area:");

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (!string.IsNullOrEmpty(message.Text))
            {
                string location = message.Text;
                context.Done(location);
            }
            else
            {
                --attempts;
                if (attempts > 0)
                {
                    await context.PostAsync("I'm sorry, I don't understand your reply. What is your age (e.g. '42')?");

                    context.Wait(this.MessageReceivedAsync);
                }
                else
                {
                    context.Fail(new TooManyAttemptsException("Message was not a valid age."));
                }
            }
        }
    }
}