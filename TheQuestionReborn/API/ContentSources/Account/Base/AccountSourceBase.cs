using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using TheQuestionReborn.Helpers;
using TheQuestionReborn.Model;

namespace TheQuestionReborn.API.ContentSources.Account.Base
{
    public abstract class AccountSourceBase
    {
        private HtmlDocument htmlDoc;
        private readonly HttpClient http;


        protected AccountSourceBase()
        {
            http = new HttpClient();
            htmlDoc = new HtmlDocument();
        }

        public async Task<ObservableCollection<QuestionModel>> GetQuestions(string urlBase, AccountModel acc, int itemsPerPage,
            int currentPage)
        {
            var questions = new ObservableCollection<QuestionModel>();

            var requestUrl = urlBase + acc.Id + "&limit=" + itemsPerPage + "&offset=" + itemsPerPage * currentPage + "&sort=date";

            htmlDoc = new HtmlDocument();

            var response = await http.GetStringAsync(requestUrl);

            var source = WebUtility.HtmlDecode(response);

            htmlDoc.LoadHtml(source);

            var parseText =
                htmlDoc.DocumentNode.Descendants()
                    .Where(
                        x =>
                            (x.Name == "div" && x.Attributes["class"] != null &&
                             x.Attributes["class"].Value.Contains("list-questions__question\\")))
                    .ToList();

            foreach (var question in parseText)
            {
                try
                {
                    var imagePath = string.Empty;

                    var title =
                        question.GetNode("h1", "class", "list-questions__header-title")
                            .InnerText.Replace("\\n", string.Empty)
                            .Replace("\\r", string.Empty);

                    var url =
                        question.Descendants().First(x => (x.Name == "a" && x.Attributes["class"] == null)).Attributes[
                            "href"].Value.Substring(1).Replace("\"", "");


                    var topicsContent = question.GetNode("div", "class", "list-questions__header-topics\\");
                    var topics = GetTopicsFromQuestion(topicsContent);

                    var imagePathNode = question.Descendants().FirstOrDefault(x => (x.Attributes["data-sizes"] != null));

                    if (imagePathNode != null)
                        imagePath = imagePathNode.Attributes["data-bgset"].Value.Substring(2);

                    var date =
                        question.GetNode("div", "class", "list-questions__date")
                            .InnerHtml.Replace("<br>", " ")
                            .TranslateMonth();
                    var counters =
                        question.Descendants()
                            .Where(
                                x =>
                                    (x.Name == "div" && x.Attributes["class"] != null &&
                                     x.Attributes["class"].Value.Contains("list-questions__header-info-counter")))
                            .ToList();
                    var viewsCount = counters[0].InnerText;
                    var answerCount = counters[1].InnerText;

                    var userAnswer = string.Empty;

                    try
                    {
                        userAnswer = question.GetNode("div", "class", "list-questions__answer-text\\").InnerText;
                    }
                    catch (Exception)
                    {
                        // ignored
                    }


                    var questionResult = new QuestionModel()
                    {
                        Id = url.Split('/')[2],
                        Title = title,
                        Url = string.Format("https://thequestion.ru{0}", url),
                        Topics = topics,
                        ImagePath = imagePath,
                        Date = date,
                        ViewsCount = viewsCount,
                        AnswersCount = answerCount,
                        PreviewAnswer = userAnswer,
                        AccountAnswer = acc

                    };

                    questions.Add(questionResult);
                }
                catch (Exception e)
                {
                    // ignored
                }
            }

            return questions;

        }

        private List<Model.Topic> GetTopicsFromQuestion(HtmlNode topicsContent)
        {
            var topics = topicsContent.Descendants().Where(x => x.Name == "a");

            return (from topic in topics
                    let title = topic.InnerText.ToUpper()
                    let url = topic.Attributes["href"].Value
                    let substr = url.Substring(8).Split('/')
                    let id = Convert.ToInt32(substr[1])
                    select new Model.Topic()
                    {
                        Title = title,
                        Url = url.Substring(1, url.Length - 3).Replace("\"", ""),
                        Id = id
                    }).ToList();
        }
    }
}
