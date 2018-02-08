using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Enums;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.FrenchDialogs
{
    [Serializable]
    public class FoodCategoriesDialog : IDialog<string>
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
            Dictionary<string, Tuple<string, Categories>> cardContentList = new Dictionary<string, Tuple<string, Categories>>();

            cardContentList.Add("North US", new Tuple<string, Categories>
                ("http://news.antiwar.com/wp-content/uploads/2012/03/obama.jpg", Categories.NorthUs));
            cardContentList.Add("Asian", new Tuple<string, Categories>
                ("http://strongertogether.coop/sites/default/files/styles/article_node/public/wp-content/uploads/2013/02/Traditional_Asian_Food_Flavorful_Healthful_0.jpg?itok=DNpSK8Zg", Categories.Asian));
            cardContentList.Add("Italian", new Tuple<string, Categories>
                ("http://www.pizzamaria-brockton.com/images/pizza_party_maria_brockton.png", Categories.Italian));
            cardContentList.Add("Greek", new Tuple<string, Categories>
                ("https://fthmb.tqn.com/nmxcf2D8rVJNEcizH6i94lvtIFc=/425x326/filters:no_upscale()/Baklava-GettyImages-183422455-58c776223df78c353c747438.jpg", Categories.Greek));
            cardContentList.Add("Iran", new Tuple<string, Categories>
                ("https://www.sassyhongkong.com/wp-content/uploads/2017/01/SHK-persian-food-tahchin-700x394.jpg", Categories.Iran));
            cardContentList.Add("Indian", new Tuple<string, Categories>(
                "https://cdn1.i-scmp.com/sites/default/files/styles/980x551/public/images/methode/2016/05/20/5987ea58-1cd7-11e6-9777-749fedcc73f5_1280x720.jpg?itok=BImrmBaj", Categories.Indian));
            cardContentList.Add("Packistan", new Tuple<string, Categories>
                ("https://www.wefindyougo.com/wp-content/uploads/2013/10/Pakistani-Food.jpg", Categories.Pakistani));
            cardContentList.Add("European", new Tuple<string, Categories>
                ("http://europeanfoodbayarea.com/wp-content/uploads/2011/11/russian_food.jpg", Categories.European));
            cardContentList.Add("Deserts", new Tuple<string, Categories>
                ("https://media-cdn.tripadvisor.com/media/photo-s/0e/1c/7e/5a/an-excellent-decert-at.jpg", Categories.Desserts));

            foreach (var cardContent in cardContentList)
            {
                List<CardImage> cardImages = new List<CardImage>();
                cardImages.Add(new CardImage(url: cardContent.Value.Item1));

                List<CardAction> cardButtons = new List<CardAction>();

                CardAction plButton = new CardAction()
                {
                    Value = $"{cardContent.Value.Item2}",
                    Type = "postBack",
                    Title = $"{cardContent.Key}"
                };

                cardButtons.Add(plButton);

                HeroCard plCard = new HeroCard()
                {
                    Title = $"{cardContent.Key}",
                    Subtitle = $"{cardContent.Key}",
                    Images = cardImages,
                    Buttons = cardButtons
                };

                Attachment plAttachment = plCard.ToAttachment();
                replyToConversation.Attachments.Add(plAttachment);
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