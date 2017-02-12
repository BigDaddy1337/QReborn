using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.Web.Syndication;
using HtmlAgilityPack;
using Microsoft.Toolkit.Uwp.Notifications;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using XmlDocument = Windows.Data.Xml.Dom.XmlDocument;

namespace TileManagment
{
    public sealed class TileManager : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // Get a deferral, to prevent the task from closing prematurely
            // while asynchronous code is still running.
            var deferral = taskInstance.GetDeferral();

            // Download the feed.
            var feedItem = await GetFeedItem();

            // Update the live tile with the feed items.
            UpdateTile(feedItem);

            // Inform the system that the task is finished.
            deferral.Complete();
        }

        private static void UpdateTile(TileQuestionModel feedItem)
        {
            // Create a tile update manager for the specified syndication feed.
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();

            var content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileWide = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            TextStacking = TileTextStacking.Center,
                            //BackgroundImage = new TileBackgroundImage()
                            //{
                            //    Source = feedItem.Url,
                            //    HintOverlay = 20
                            //},
                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = feedItem.Title,
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                    HintAlign = AdaptiveTextAlign.Center,
                                    HintWrap = true
                                }
                            }
                        }
                    }
                }
            };

            updater.Update(new TileNotification(content.GetXml()));
        }

        private async Task<TileQuestionModel> GetFeedItem()
        {
            var requestUrl = "https://thequestion.ru/lists/new/questions?limit=1&offset=0&region=ru&sort=answer&sortItems=%7B%22name%22:%22%D0%9F%D0%BE+%D0%B4%D0%B0%D1%82%D0%B5%22,%22value%22:%22date%22%7D&sortItems=%7B%22name%22:%22%D0%9F%D0%BE+%D0%BD%D0%BE%D0%B2%D1%8B%D0%BC+%D0%BE%D1%82%D0%B2%D0%B5%D1%82%D0%B0%D0%BC%22,%22value%22:%22answer%22%7D&sortItems=%7B%22name%22:%22%D0%9F%D0%BE+%D0%BF%D0%BE%D0%BF%D1%83%D0%BB%D1%8F%D1%80%D0%BD%D0%BE%D1%81%D1%82%D0%B8%22,%22value%22:%22followers%22%7D&sortItems=%7B%22name%22:%22%D0%9F%D0%BE+%D0%BF%D1%80%D0%BE%D1%81%D0%BC%D0%BE%D1%82%D1%80%D0%B0%D0%BC%22,%22value%22:%22views%22%7D&type=feed";

            var http = new HttpClient();
            TileQuestionModel item = null;

            http.DefaultRequestHeaders.Referrer = new Uri("http://thequestion.ru/all?qsort=answer");
            http.DefaultRequestHeaders.Host = "thequestion.ru";

            var response = await http.GetStringAsync(requestUrl);

            var source = WebUtility.HtmlDecode(response);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(source);

            var parseText = htmlDoc.DocumentNode.Descendants().Where(x => (x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value.Contains("list-questions__question-new-wrapper\\"))).ToList();

            foreach (var question in parseText)
            {
                var imagePath = string.Empty;
                var title = GetNode(question, "a", "class", "list-questions__question-new-title").InnerText.Replace("\\n", string.Empty).Replace("\\r", string.Empty);
                var imagePathNode = question.Descendants().FirstOrDefault(x => (x.Attributes["data-sizes"] != null));

                if (imagePathNode != null)
                    imagePath = imagePathNode.Attributes["data-bgset"].Value.Substring(2);

                item = new TileQuestionModel()
                {
                    Title = title,
                    Url = imagePath
                };

            }

            return item;
        }

        private static HtmlNode GetNode(HtmlNode nodes, string name, string attribute, string attributeValue)
        {
            return nodes.Descendants().FirstOrDefault(x => (x.Name == name && x.Attributes[attribute] != null && x.Attributes[attribute].Value.Contains(attributeValue)));
        }
    }
}
