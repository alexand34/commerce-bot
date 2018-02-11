using System;
using System.Threading.Tasks;
using Bot.Enums;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.Root
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private Languages _language;
        private int _category;
        private string _location;
        private int _restaurant;
        private SortingType _sortingType;
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
            await context.PostAsync($"👋Hi there,🤖 my name is Tastebot and I’m here to help" +
                                    $" you to find the most delicious 😋 food in Montreal. Just " +
                                    $"tell me what you want to try this time 🍽 and I’ll give you" +
                                    $" the best options I have at the moment.");

            context.Call(new LanguageDialog(_language), this.LanguagesDialogResumeAfter);
        }

        private async Task LanguagesDialogResumeAfter(IDialogContext context, IAwaitable<Languages> result)
        {
            try
            {
                this._language = await result;

                if(_language == Languages.French)
                    context.Call(new FrenchDialogs.FoodCategoriesDialog(), this.FoodCategoriesDialogResumeAfter);
                else
                    context.Call(new EnglishDialogs.FoodCategoriesDialog(), this.FoodCategoriesDialogResumeAfter);
            }
            catch (Exception ex)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");

                await this.SendWelcomeMessageAsync(context);
            }
        }

        private async Task FoodCategoriesDialogResumeAfter(IDialogContext context, IAwaitable<int> result)
        {
            try
            {
                this._category = await result;

                if(_language == Languages.French)
                    context.Call(new FrenchDialogs.LocationDialog(), this.LocationDialogResumeAfter);
                else
                    context.Call(new EnglishDialogs.LocationDialog(), this.LocationDialogResumeAfter);
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
                this._location = await result;

                if (_language == Languages.French)
                    context.Call(new FrenchDialogs.SortingDialog(), this.SortingDialogResumeAfter);
                else
                    context.Call(new EnglishDialogs.SortingDialog(), this.SortingDialogResumeAfter);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");
            }
           
        }

        private async Task SortingDialogResumeAfter(IDialogContext context, IAwaitable<SortingType> result)
        {
            try
            {
                this._sortingType = await result;
                if (_language == Languages.French)
                    context.Call(new FrenchDialogs.RestaurantDialog(_location, _category, _sortingType), this.RestaurantDialogResumeAfter);
                else
                    context.Call(new EnglishDialogs.RestaurantDialog(_location, _category, _sortingType), this.RestaurantDialogResumeAfter);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");
            }
        }

        private async Task RestaurantDialogResumeAfter(IDialogContext context, IAwaitable<int> result)
        {
            try
            {
                this._restaurant = await result;

            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");
            }
            finally
            {
                await context.PostAsync($"Category: { _category }. Location: { _location }.\n" +
                                        $" User: {context.Activity.From.Name} \n Id: {context.Activity.From.Id} \n Properties: {context.Activity.From.Properties} + \n {_restaurant}");
            }
        }
    }
}