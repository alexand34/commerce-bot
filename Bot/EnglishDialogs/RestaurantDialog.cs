﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using Bot.Enums;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.EnglishDialogs
{
    [Serializable]
    public class RestaurantDialog : IDialog<int>
    {
        private string _location;
        private readonly int _categoryId;
        private double _distance;
        private SortingType _sortingType;
        public RestaurantDialog(string location, int categoryId, SortingType sortingType)
        {
            _location = location;
            _categoryId = categoryId;
            _sortingType = sortingType;
        }

        public async Task StartAsync(IDialogContext context)
        {
            List<Restaurant> restaurantsByCategory = new List<Restaurant>();
            List<Restaurant> restaurantsToShow = new List<Restaurant>();
            using (Entities ctx = new Entities())
            {
                restaurantsByCategory.AddRange(ctx.Restaurants.Where(x => x.CategoryId == _categoryId).ToList());
            }

            if (restaurantsByCategory.Count != 0)
            {
                foreach (var restaurant in restaurantsByCategory)
                {

                    string url = @"http://maps.googleapis.com/maps/api/distancematrix/xml?origins=" + restaurant.RestaurantAddress.Replace(" ", "+") + "&destinations=" + _location.Replace(" ", "+") + "&sensor=false";

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    WebResponse response = request.GetResponse();
                    Stream dataStream = response.GetResponseStream();
                    StreamReader sreader = new StreamReader(dataStream);
                    string responsereader = sreader.ReadToEnd();
                    response.Close();

                    DataSet ds = new DataSet();
                    ds.ReadXml(new XmlTextReader(new StringReader(responsereader)));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables["element"].Rows[0]["status"].ToString() == "OK")
                        {
                            Double.TryParse(ds.Tables["distance"].Rows[0]["text"].ToString(), out _distance);
                        }
                    }

                    if (_distance < 8.0)
                    {
                        restaurantsToShow.Add(restaurant);
                    }
                }

            }

            if (restaurantsToShow.Count == 0)
            {
                var replyToConversation = context.MakeMessage();
                replyToConversation.Text = "We did not find anything suitable for you. Please, try different location.";
                await context.PostAsync(replyToConversation);
            }
            else
            {
                var result = new List<Restaurant>();
                switch (_sortingType)
                {
                    case SortingType.ByPrice:
                        result = restaurantsToShow.OrderBy(x => x.AverageReceipt).ToList();
                        break;
                    case SortingType.ByRating:
                        result = restaurantsToShow.OrderBy(x => x.Rating).ToList();
                        break;
                    case SortingType.ByDistance:
                        result = restaurantsToShow.OrderBy(x => x.RestaurantAddress).ToList();
                        break;
                }
                var replyToConversation = context.MakeMessage();//.CreateReply("Should go to conversation, in carousel format");
                replyToConversation.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                replyToConversation.Attachments = new List<Attachment>();
                replyToConversation.Text = "Great, here are the top picks for you:";
                using (Entities ctx = new Entities())
                {
                    foreach (var restaurant in result)
                    {
                        List<CardImage> cardImages = new List<CardImage>();
                        cardImages.Add(new CardImage(url: "https://assets.change.org/photos/2/zf/ml/fFZFmLnDFZmgAUn-128x128-noPad.jpg?1453750742"));

                        List<CardAction> cardButtons = new List<CardAction>();

                        CardAction plButton = new CardAction()
                        {
                            Value = $"{restaurant.Id}",
                            Type = "imBack",
                            Title = $"{restaurant.RestaurantName}"
                        };

                        cardButtons.Add(plButton);

                        HeroCard plCard = new HeroCard()
                        {
                            Title = $"{restaurant.RestaurantName}",
                            Subtitle = $"{restaurant.RestaurantName}",
                            Images = cardImages,
                            Buttons = cardButtons
                        };

                        Attachment plAttachment = plCard.ToAttachment();
                        replyToConversation.Attachments.Add(plAttachment);
                    }
                }

                await context.PostAsync(replyToConversation);
            }

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (!string.IsNullOrEmpty(message.Text))
            {
                string location = message.Text;
                context.Done(location);
            }
            else
            {
                await context.PostAsync("I'm sorry, I don't understand your reply. Lets try again?");

                context.Wait(this.MessageReceivedAsync);
            }
        }
    }
}