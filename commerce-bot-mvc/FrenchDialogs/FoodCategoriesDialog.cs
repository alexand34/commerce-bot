using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commerce_bot_mvc.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace commerce_bot_mvc.FrenchDialogs
{
    [Serializable]
    public class FoodCategoriesDialog : IDialog<int>
    {
        public async Task StartAsync(IDialogContext context)
        {
            //await context.PostAsync("What category of food do you prefer today?");
            await GetFoodCategories(context);
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task GetFoodCategories(IDialogContext context)
        {
            var replyToConversation = context.MakeMessage();//.CreateReply("Should go to conversation, in carousel format");
            replyToConversation.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            replyToConversation.Attachments = new List<Attachment>();
            replyToConversation.Text = "Please, choose your food category.";
            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {
                foreach (var category in ctx.Categories)
                {
                    List<CardImage> cardImages = new List<CardImage>();
                    cardImages.Add(new CardImage(url: "https://image.flaticon.com/icons/png/128/164/164826.png"));

                    List<CardAction> cardButtons = new List<CardAction>();

                    CardAction plButton = new CardAction()
                    {
                        Value = $"{category.Id}",
                        Type = "postBack",
                        Title = $"{category.CategoryName}"
                    };

                    cardButtons.Add(plButton);

                    HeroCard plCard = new HeroCard()
                    {
                        Title = $"{category.CategoryName}",
                        Subtitle = $"{category.CategoryName}",
                        Images = cardImages,
                        Buttons = cardButtons
                    };

                    Attachment plAttachment = plCard.ToAttachment();
                    replyToConversation.Attachments.Add(plAttachment);
                }
            }

            await context.PostAsync(replyToConversation);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            /* If the message returned is a valid name, return it to the calling dialog. */
            if ((message.Text != null) && (message.Text.Trim().Length > 0))
            {
                /* Completes the dialog, removes it from the dialog stack, and returns the result to the parent/calling
                    dialog. */
                context.Done(Int32.Parse(message.Text));
            }
            /* Else, try again by re-prompting the user. */
            else
            {
                await context.PostAsync("I'm sorry, I don't understand your reply. What is your name (e.g. 'Bill', 'Melinda')?");

                context.Wait(this.MessageReceivedAsync);
            }
        }
    }
}