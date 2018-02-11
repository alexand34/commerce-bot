using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Enums;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.FrenchDialogs
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
            replyToConversation.Text = "SVP, choissiez votre type d’aliments.";
            using (Entities ctx = new Entities())
            {
                foreach (var category in ctx.Categories)
                {
                    List<CardImage> cardImages = new List<CardImage>();
                    cardImages.Add(new CardImage(url: "https://p.memecdn.com/avatars/s_28212_50048351785dc.jpg"));

                    List <CardAction> cardButtons = new List<CardAction>();

                    CardAction plButton = new CardAction()
                    {
                        Value = $"{category.FrenchCategoryName}",
                        Type = "imBack",
                        Title = $"{category.FrenchCategoryName}"
                    };

                    cardButtons.Add(plButton);

                    HeroCard plCard = new HeroCard()
                    {
                        Title = $"{category.FrenchCategoryName}",
                        Subtitle = $"{category.FrenchCategoryName}",
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
                context.Done(message.Text);
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