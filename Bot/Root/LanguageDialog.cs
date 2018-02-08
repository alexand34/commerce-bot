using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Enums;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.Root
{
    [Serializable]
    public class LanguageDialog : IDialog<Languages>
    {
        private Languages _language;

        public LanguageDialog(Languages language)
        {
            this._language = language;
        }

        public async Task StartAsync(IDialogContext context)
        {
            var replyToConversation = context.MakeMessage();
            replyToConversation.AttachmentLayout = AttachmentLayoutTypes.List;
            replyToConversation.Attachments = new List<Attachment>();
            replyToConversation.Text = "Please, choose your language";
            replyToConversation.Attachments.Add(GetLanguageButtonsCard());

            await context.PostAsync(replyToConversation);

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (!string.IsNullOrEmpty(message.Text))
            {
                this._language = message.Text.ToLower().Equals("français") 
                    || message.Text.ToLower().Equals("french")
                    ? Languages.French : Languages.English;

                context.Done(_language);
            }
            else
            {
                var replyToConversation = context.MakeMessage();
                replyToConversation.AttachmentLayout = AttachmentLayoutTypes.List;
                replyToConversation.Attachments = new List<Attachment>();
                replyToConversation.Text = "Please, choose your language";
                replyToConversation.Attachments.Add(GetLanguageButtonsCard());

                await context.PostAsync(replyToConversation);

                context.Wait(this.MessageReceivedAsync);
            }
        }

        private Attachment GetLanguageButtonsCard()
        {
            var buttons = CreateButtons();
            HeroCard langCard = new HeroCard()
            {
                Buttons = buttons,
            };

            return langCard.ToAttachment();
        }

        private List<CardAction> CreateButtons()
        {
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction EnglishButton = new CardAction()
            {
                Type = "imBack",
                Title = "English",
                Value = "English"
            };
            CardAction FrenchButton = new CardAction()
            {
                Type = "imBack",
                Title = "Français",
                Value = "français"
            };
            cardButtons.Add(EnglishButton);
            cardButtons.Add(FrenchButton);

            return cardButtons;
        }
    }
}