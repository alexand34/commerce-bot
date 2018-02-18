﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Enums;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.Root
{
    [Serializable]
    public class LanguageDialog : IDialog<Tuple<Languages, string>>
    {
        private Languages _language;

        public LanguageDialog(Languages language)
        {
            this._language = language;
        }

        public async Task StartAsync(IDialogContext context)
        {
            var user = new User();
            using (Entities ctx = new Entities())
            {
                user = ctx.Users.FirstOrDefault(x => x.MessengerId == context.Activity.From.Id);
                if (user != null)
                {
                    _language = user.Language == (int)Languages.English ? Languages.English : Languages.French;
                    context.Done(new Tuple<Languages, string>(_language, context.Activity.From.Id));
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
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (!string.IsNullOrEmpty(message.Text))
            {
                this._language = message.Text.ToLower().Equals("français")
                                 || message.Text.ToLower().Equals("french")
                    ? Languages.French : Languages.English;
                using (Entities ctx = new Entities())
                {
                    ctx.Users.Add(new User()
                    {
                        UserName = context.Activity.From.Name,
                        MessengerId = context.Activity.From.Id,
                        Language = (int)_language
                    });
                    ctx.SaveChanges();
                }

                context.Done(new Tuple<Languages, string>(_language, context.Activity.From.Id));
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