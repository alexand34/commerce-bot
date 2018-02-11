using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Enums;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.EnglishDialogs
{
    [Serializable]
    public class SortingDialog : IDialog<SortingType>
    {
        public SortingDialog()
        {
        }

        public async Task StartAsync(IDialogContext context)
        {
            var replyToConversation = context.MakeMessage();
            replyToConversation.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            replyToConversation.Attachments = new List<Attachment>();

            var actions = new List<CardAction>();
            actions.Add(new CardAction
            {
                Title = "By price",
                Text = "By price",
                Value = $"{SortingType.ByPrice}",
                Type = "postBack",
            });
            actions.Add(new CardAction
            {
                Title = "By rating",
                Text = "By rating",
                Value = $"{SortingType.ByRating}",
                Type = "postBack",
            });
            actions.Add(new CardAction
            {
                Title = "By distance",
                Text = "By distance",
                Value = $"{SortingType.ByDistance}",
                Type = "postBack",
            });

            replyToConversation.Attachments.Add(
                new HeroCard
                {
                    Title = "Choose option",
                    Buttons = actions
                }.ToAttachment()
            );
            await context.PostAsync(replyToConversation);
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (!string.IsNullOrEmpty(message.Text))
            {
                SortingType sortingType = SortingType.ByPrice;
                if (message.Text.Contains("Price"))
                {
                    sortingType = SortingType.ByPrice;
                }
                else if (message.Text.Contains("Rating"))
                {
                    sortingType = SortingType.ByRating;
                }
                else
                {
                    sortingType = SortingType.ByDistance;
                }
                context.Done(sortingType);
            }
            else
            {
                await context.PostAsync("I'm sorry, I don't understand your reply. Lets try again?");

                context.Wait(this.MessageReceivedAsync);
            }
        }
    }
}