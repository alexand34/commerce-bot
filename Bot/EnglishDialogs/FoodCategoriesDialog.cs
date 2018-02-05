using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.Dialogs
{
    [Serializable]
    public class FoodCategoriesDialog : IDialog<string>
    {
        private int attempts = 3;

        public async Task StartAsync(IDialogContext context)
        {
            //await context.PostAsync("What category of food do you prefer today?");
            GetFoodCategories(context);
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task GetFoodCategories(IDialogContext context)
        {
            var replyToConversation = context.MakeMessage();//.CreateReply("Should go to conversation, in carousel format");
            replyToConversation.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            replyToConversation.Attachments = new List<Attachment>();
            replyToConversation.Text = "What category of food do you prefer today ?";
            Dictionary<string, string> cardContentList = new Dictionary<string, string>();
            cardContentList.Add("North US", "http://news.antiwar.com/wp-content/uploads/2012/03/obama.jpg");
            cardContentList.Add("Asian", "http://strongertogether.coop/sites/default/files/styles/article_node/public/wp-content/uploads/2013/02/Traditional_Asian_Food_Flavorful_Healthful_0.jpg?itok=DNpSK8Zg");
            cardContentList.Add("Italian", "http://www.pizzamaria-brockton.com/images/pizza_party_maria_brockton.png");
            cardContentList.Add("Greek", "https://fthmb.tqn.com/nmxcf2D8rVJNEcizH6i94lvtIFc=/425x326/filters:no_upscale()/Baklava-GettyImages-183422455-58c776223df78c353c747438.jpg");
            cardContentList.Add("Iran", "https://www.sassyhongkong.com/wp-content/uploads/2017/01/SHK-persian-food-tahchin-700x394.jpg");
            cardContentList.Add("Indian", "https://cdn1.i-scmp.com/sites/default/files/styles/980x551/public/images/methode/2016/05/20/5987ea58-1cd7-11e6-9777-749fedcc73f5_1280x720.jpg?itok=BImrmBaj");
            cardContentList.Add("Packistan", "https://www.wefindyougo.com/wp-content/uploads/2013/10/Pakistani-Food.jpg");
            cardContentList.Add("European", "http://europeanfoodbayarea.com/wp-content/uploads/2011/11/russian_food.jpg");
            cardContentList.Add("Deserts", "https://media-cdn.tripadvisor.com/media/photo-s/0e/1c/7e/5a/an-excellent-decert-at.jpg");

            foreach (KeyValuePair<string, string> cardContent in cardContentList)
            {
                List<CardImage> cardImages = new List<CardImage>();
                cardImages.Add(new CardImage(url: cardContent.Value));

                List<CardAction> cardButtons = new List<CardAction>();

                CardAction plButton = new CardAction()
                {
                    Value = $"{cardContent.Key}",
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
                --attempts;
                if (attempts > 0)
                {
                    await context.PostAsync("I'm sorry, I don't understand your reply. What is your name (e.g. 'Bill', 'Melinda')?");

                    context.Wait(this.MessageReceivedAsync);
                }
                else
                {
                    /* Fails the current dialog, removes it from the dialog stack, and returns the exception to the 
                        parent/calling dialog. */
                    context.Fail(new TooManyAttemptsException("Message was not a string or was an empty string."));
                }
            }
        }


    }
}