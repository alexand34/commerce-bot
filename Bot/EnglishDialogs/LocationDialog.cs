using System;
using System.Threading.Tasks;
using Bot.Enums;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.EnglishDialogs
{
    [Serializable]
    public class LocationDialog : IDialog<string>
    {

        public LocationDialog()
        {
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"Okay, the category is set. " +
                                    $"Please, provide us your accurate address for us to pick the" +
                                    $" most suitable restaurants.");

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
                await context.PostAsync("I'm sorry, I don't understand your reply. Lets try again?");

                context.Wait(this.MessageReceivedAsync);
            }
        }
    }
}