using System;
using System.Threading.Tasks;
using Bot.Dialogs;
using Bot.Enums;
using Bot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.EnglishDialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private User user = new User();
        private string Category;
        private string Location;
        private Languages Language;

        public async Task StartAsync(IDialogContext context)
        {
            /* Wait until the first message is received from the conversation and call MessageReceviedAsync 
             *  to process that message. */
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            /* When MessageReceivedAsync is called, it's passed an IAwaitable<IMessageActivity>. To get the message,
             *  await the result. */
            var message = await result;

            await this.SendWelcomeMessageAsync(context);
        }

        private async Task SendWelcomeMessageAsync(IDialogContext context)
        {
            await context.PostAsync("👋Hi there,🤖 my name is Tastebot and I’m here to help you to find the most delicious 😋 food in Montreal. Just tell me what you want to try this time 🍽 and I’ll give you the best options I have at the moment.");

            context.Call(new FoodCategoriesDialog(), this.LanguagesDialogResumeAfter);
        }

        private async Task LanguagesDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                this.Category = await result;

                context.Call(new LocationDialog(this.Category), this.LocationDialogResumeAfter);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");

                await this.SendWelcomeMessageAsync(context);
            }
        }

        private async Task FoodCategoriesDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                this.Category = await result;

                context.Call(new LocationDialog(this.Category), this.LocationDialogResumeAfter);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");

                await this.SendWelcomeMessageAsync(context);
            }
        }

        private async Task LocationDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                this.Location = await result;

                await context.PostAsync($"Location: { Category }. Location: { Location }.");

            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");
            }
            finally
            {
                //await this.SendWelcomeMessageAsync(context);
            }
        }
    }
}