using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commerce_bot_mvc.Enums;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace commerce_bot_mvc.FrenchDialogs
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
                Value = $"{(int)SortingType.ByPrice}"
            });
            actions.Add(new CardAction
            {
                Title = "By rating",
                Text = "By rating",
                Value = $"{(int)SortingType.ByRating}"
            });
            actions.Add(new CardAction
            {
                Title = "By distance",
                Text = "By distance",
                Value = $"{(int)SortingType.ByDistance}"
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
                SortingType sortingType = (SortingType)Int32.Parse(message.Text);
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