using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TheQuestionReborn.Helpers;
using TheQuestionReborn.Model;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace TheQuestionReborn.API
{
    public class ContentFactory
    {
        public async Task GetFeedData(int itemsPerPage, int currentPage)
        {
            var requestUrl = "https://thequestion.ru/lists/new/questions?limit=" + itemsPerPage + "&offset=" + currentPage * itemsPerPage + "&region=ru&sort=answer&sortItems=%7B%22name%22:%22%D0%9F%D0%BE+%D0%B4%D0%B0%D1%82%D0%B5%22,%22value%22:%22date%22%7D&sortItems=%7B%22name%22:%22%D0%9F%D0%BE+%D0%BD%D0%BE%D0%B2%D1%8B%D0%BC+%D0%BE%D1%82%D0%B2%D0%B5%D1%82%D0%B0%D0%BC%22,%22value%22:%22answer%22%7D&sortItems=%7B%22name%22:%22%D0%9F%D0%BE+%D0%BF%D0%BE%D0%BF%D1%83%D0%BB%D1%8F%D1%80%D0%BD%D0%BE%D1%81%D1%82%D0%B8%22,%22value%22:%22followers%22%7D&sortItems=%7B%22name%22:%22%D0%9F%D0%BE+%D0%BF%D1%80%D0%BE%D1%81%D0%BC%D0%BE%D1%82%D1%80%D0%B0%D0%BC%22,%22value%22:%22views%22%7D&type=feed";

            var http = new HttpClient();
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
                var content = question.GetNode("div", "class", "list-questions__question-new-content\\");
                var title = question.GetNode("a", "class", "list-questions__question-new-title").InnerText.Replace("\\n", string.Empty).Replace("\\r", string.Empty);
                var userName = question.GetNode("a", "class", "list-questions__question-new-username\\").InnerText;
                var url = question.GetNode("a", "class", "list-questions__question-new-title").Attributes["href"].Value.Substring(1).Replace("\"", ""); ;
                var topics = GetTopicsFromQuestion(content);

                var imagePathNode = question.Descendants().FirstOrDefault(x => (x.Attributes["data-sizes"] != null));

                if (imagePathNode != null)
                    imagePath = imagePathNode.Attributes["data-bgset"].Value.Substring(2);

                var date = question.GetNode("div", "class", "list-questions__question-new-date\\").InnerText.TranslateMonth();
                var counters = question.Descendants().Where(x => (x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value.Contains("list-questions__question-new-info-counter\\"))).ToList();
                var viewsCount = counters[0].InnerText;
                var answerCount = counters[1].InnerText;

                var questionResult = new QuestionModel()
                {
                    Id = url.Split('/')[2],
                    Title = title,
                    Username = userName,
                    Url = string.Format("https://thequestion.ru{0}", url),
                    Topics = topics,
                    ImagePath = imagePath,
                    Date = date,
                    ViewsCount = viewsCount,
                    AnswersCount = answerCount
                };

                ApplicationData.Feed.Add(questionResult);
            }
        }

        /// <summary>
        /// Парсинг тем при получении ленты
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public List<Topic> GetTopicsFromQuestion(HtmlNode content)
        {
            var topicsList = new List<Topic>();
            var topicsContent = content.GetNode("div", "class", "list-questions__question-new-topics\\");
            var topics = topicsContent.Descendants().Where(x => x.Name == "a");
            foreach (var topic in topics)
            {
                var title = topic.InnerText.ToUpper();
                var url = topic.Attributes["href"].Value;
                var substr = url.Substring(8).Split('/');
                var id = Convert.ToInt32(substr[1]);

                topicsList.Add(new Topic()
                {
                    Title = title,
                    Url = url.Substring(1, url.Length - 3).Replace("\"", ""),
                    Id = id
                });

            }

            return topicsList;
        }

        public async Task<List<AnswerModel>> GetQuestionBody(QuestionModel question)
        {
            var http = new HttpClient();
            http.DefaultRequestHeaders.Host = "thequestion.ru";
            question.Answers = new List<AnswerModel>();
            var response = await http.GetStringAsync(question.Url);

            var source = WebUtility.HtmlDecode(response);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(source);

            var answers = htmlDoc.DocumentNode.Descendants().Where(x => (x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value == "answer__wrapper")).ToList();//GetNode("div", "class", "answer__wrapper");

            foreach (var answer in answers)
            {
                try
                {
                   
                    var answerDate = answer.GetNode("div", "class", "answer__date").InnerHtml.Replace("<br>", " ").TranslateMonth();
                    var answerAccount = GetAnswerAccount(answer);
                    var qml = answer.GetNode("qml", "class", "answer__text");
                    var id = qml.Attributes["id"].Value.Split('-')[1];
                    var text = qml.InnerHtml;
                    var votes = answer.GetNode("div", "class", "answer__votes-value").InnerText;
                    var comments = answer.Descendants().FirstOrDefault(x => (x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value == "value")).InnerText;

                    var answerModel = new AnswerModel()
                    {
                        Id = id,
                        User = answerAccount,
                        Text = text,
                        Votes = votes,
                        Date = answerDate,
                        Comments = comments,
                    };

                    question.Answers.Add(answerModel);
                }
                catch (Exception e)
                {
                    // ignored
                }
            }

            return question.Answers;
        }

        private AccountModel GetAnswerAccount(HtmlNode answer)
        {
            try
            {
                var answerAccount = answer.GetNode("div", "class", "answer__account");
                var userPick = answerAccount.Descendants().FirstOrDefault(x => (x.Name == "a" && x.Attributes["data-bgset"] != null)).Attributes["data-bgset"].Value;
                var userInfoNode = answer.GetNode("a", "class", "answer__account-username");
                var userUrl = userInfoNode.Attributes["href"].Value;
                var userName = userInfoNode.InnerHtml.Split(new[] { "<small>" }, 2, StringSplitOptions.None).First();
                var userRating = userInfoNode.InnerHtml.Split(new[] { "<small>" }, 2, StringSplitOptions.None)[1].Replace("</small>", string.Empty);
                var userOccupation = answer.GetNode("div", "class", "answer__account-occupation").InnerText;

                var user = new AccountModel()
                {
                    UserName = userName,
                    ImagePath = userPick,
                    Url = string.Format("https://thequestion.ru{0}", userUrl),
                    Rating = userRating,
                    Info = userOccupation,
                    Id = userUrl.Split('/')[2]
                };

                return user;
            }
            catch (Exception e)
            {
                return new AccountModel();
            }
        }

        public async Task GetTopicsData(int itemsPerPage, int currentPage)
        {

            try
            {
                var requestUrl = "https://thequestion.ru/topics/data?order=followers&offset=" + currentPage * itemsPerPage;

                var http = new HttpClient();
                http.DefaultRequestHeaders.Referrer = new Uri("http://thequestion.ru/all?qsort=answer");
                http.DefaultRequestHeaders.Host = "thequestion.ru";

                var response = await http.GetStringAsync(requestUrl);

                var source = WebUtility.HtmlDecode(response);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(source);

                var parseHtml = htmlDoc.DocumentNode.Descendants().Where(x => (x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value.Contains("list-topics__item\\"))).ToList();

                foreach (var topic in parseHtml)
                {
                    var url = topic.Descendants().First(x => x.Name == "a" && x.Attributes["href"] != null).Attributes["href"].Value.Split('\"')[1];
                    var title = topic.GetNode("h3", "class", "list-topics__info-title\\").InnerText;
                    var countQuestion = topic.GetNode("h4", "class", "list-topics__info-questions\\").InnerText;
                    var color = topic.Attributes["style"].Value.Substring(20, 6);
                    var id = Convert.ToInt32(url.Substring(7).Split('/').First());
                    await ApplicationData.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {

                        var topicResult = new Topic()
                        {
                            Url = "thequestion.ru" + url,
                            Title = title,
                            CountOfQuestions = countQuestion,
                            ColorStr = color,
                            Id = id
                        };

                        ApplicationData.Topics.Add(topicResult);
                    });
                }
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        public async Task GetTopicFeedData(int itemsPerPage, int currentPage)
        {
            var requestUrl = "http://thequestion.ru/lists/questions?filter=answered&limit=" + itemsPerPage + "&offset=" + itemsPerPage * currentPage + "&sort=date&topicId=" + ApplicationData.Topic.Id;
            var http = new HttpClient();
            http.DefaultRequestHeaders.Referrer = new Uri("http://thequestion.ru/all?qsort=answer");
            http.DefaultRequestHeaders.Host = "thequestion.ru";

            var response = await http.GetStringAsync(requestUrl);

            var source = WebUtility.HtmlDecode(response);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(source);

            var parseText = htmlDoc.DocumentNode.Descendants().Where(x => (x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value.Contains("list-questions__question\\"))).ToList();

            foreach (var question in parseText)
            {
                try
                {
                    var imagePath = string.Empty;
                    var content = question.GetNode("div", "class", "list-questions__header-center\\");
                    var title = content.ChildNodes[1].InnerText.Replace("\\n", string.Empty).Replace("\\r", string.Empty);
                    var userName = content.FirstChild.InnerText;
                    var url = content.ChildNodes[1].Attributes["href"].Value.Substring(1).Replace("\"", "");
                    var topics = GetTopicsFromTopicQuestion(content);


                    var imagePathNode = question.Descendants().FirstOrDefault(x => (x.Attributes["data-sizes"] != null));

                    if (imagePathNode != null)
                        imagePath = imagePathNode.Attributes["data-bgset"].Value.Substring(2);

                    var date = question.FirstChild.ChildNodes.Count < 2 ? question.ChildNodes[1].ChildNodes[2].InnerText.TranslateMonth() : question.FirstChild.ChildNodes[1].InnerText.TranslateMonth();

                    var counters = question.Descendants().First(x => (x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value.Contains("list-questions__header-menu-left\\")));
                    var viewsCount = counters.FirstChild.ChildNodes[1].InnerText;
                    var answerCount = counters.FirstChild.ChildNodes[3].InnerText;

                    var questionResult = new QuestionModel()
                    {
                        Id = url.Split('/')[2],
                        Title = title,
                        Username = userName,
                        Url = string.Format("https://thequestion.ru{0}", url),
                        Topics = topics,
                        ImagePath = imagePath,
                        Date = date,
                        ViewsCount = viewsCount,
                        AnswersCount = answerCount
                    };

                    ApplicationData.TopicFeed.Add(questionResult);
                }
                catch (Exception e)
                {
                    // ignored
                }
            }

        }

        private List<Topic> GetTopicsFromTopicQuestion(HtmlNode content)
        {
            var topicsList = new List<Topic>();
            var topics = content.LastChild.ChildNodes;
            foreach (var topic in topics)
            {
                var title = topic.InnerText.ToUpper();
                var url = topic.FirstChild.Attributes["href"].Value;
                var substr = url.Substring(8).Split('/');
                var id = Convert.ToInt32(substr[1]);
                topicsList.Add(new Topic() { Title = title, Url = url.Substring(1, url.Length - 3).Replace("\"", ""), Id = id });
            }

            return topicsList;
        }


        public async Task<List<CommentModel>> GetCommentsFromAnswer(string questionId, string answerId)
        {
            var commentsList = new List<CommentModel>();

            var url = "https://thequestion.ru/questions/data/comments/" + questionId + "/" + answerId;
            var http = new HttpClient();

            var response = await http.GetStringAsync(url);

            var source = WebUtility.HtmlDecode(response);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(source);

            var comments = htmlDoc.DocumentNode.Descendants().Where(x => (x.Name == "div" && x.Attributes["id"] != null && x.Attributes["class"].Value.Contains("comment-block\\"))).ToList();

            foreach(var comment in comments)
            {
                var authorNode = comment.GetNode("a", "class", "comment-block-author-userpic\\");

                var authorPic = authorNode.Attributes["style"].Value.Substring(23).Split(')')[0];
                var authorUrl = authorNode.Attributes["href"].Value.Substring(2).TrimEnd('"');
                var authorName = comment.GetNode("a", "class", "comment-block-author-username\\").InnerText;

                var date = comment.GetNode("div", "class", "comment-block-date").InnerText;

                var text = comment.Descendants().FirstOrDefault(x => x.Name == "qml").FirstChild.InnerText;

                var votes = comment.GetNode("div", "class", "comment__votes-value\\").InnerText;

                var commentModel = new CommentModel()
                {
                    Author = new AccountModel()
                    {
                        UserName = authorName,
                        ImagePath = authorPic,
                        Url = string.Format("https://thequestion.ru{0}", authorUrl)
                    },

                    Date = date,
                    Text = text,
                    Votes = votes
                };

                commentsList.Add(commentModel);
            }
            return commentsList;
        }
    }
}