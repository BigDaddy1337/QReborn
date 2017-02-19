using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Core;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using TheQuestionReborn.Helpers;
using TheQuestionReborn.Model;

namespace TheQuestionReborn.API.ContentSources.Feed
{
    public class PopularFeedSource : IIncrementalSource<QuestionModel>
    {
        private HttpClient http;
        private HtmlDocument htmlDoc;

        public async Task<IEnumerable<QuestionModel>> GetPagedItems(int pageSize, int pageIndex)
        {
            await ApplicationData.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ApplicationData.IsDataFeedPopularLoadingAnimation.Begin();
            });

            await GetPopularFeedData(pageSize, pageIndex);

            await ApplicationData.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ApplicationData.IsDataFeedPopularLoadingAnimation.Stop();
            });

            return await Task.Run(() =>
            {
                var result = (from c in ApplicationData.PopularFeed
                              select c).Skip(pageIndex * pageSize).Take(pageSize);

                return result;
            });
        }

        public async Task GetPopularFeedData(int itemsPerPage, int currentPage)
        {

            var requestUrl = "https://thequestion.ru/lists/questions/likes?limit=" + itemsPerPage + "&offset=" + currentPage * itemsPerPage + "&region=ru&period=" + ApplicationData.PopularFeedPeriod;

            http = new HttpClient();
            htmlDoc = new HtmlDocument();

            var response = await http.GetStringAsync(requestUrl);

            var source = WebUtility.HtmlDecode(response);

            htmlDoc.LoadHtml(source);

            var questionsSplitted = htmlDoc.DocumentNode.Descendants().Where(x => (x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value.Contains("list-questions-ratings__item\\"))).ToList();

            foreach(var question in questionsSplitted)
            {
                var title = question.GetNode("a", "class", "list-questions-ratings__item-title\\").InnerText;

                try
                {
                    var questionResult = await GetMoreInfo(title.Replace("\\n", string.Empty).Replace("\\r", string.Empty));
                    ApplicationData.PopularFeed.Add(questionResult);
                }
                catch (Exception e)
                {
                    // ignored
                }
            }

        }


        public async Task<QuestionModel> GetMoreInfo(string title)
        {
            var url = "https://thequestion.ru/search/questions?limit=1&offset=0&q=" + title;

            var response = await http.GetStringAsync(url);
            var results = JObject.Parse(response);

            var item = results["items"][0];

            var account = item["account"];
            var userName = account.Value<string>("firstName") + " " + account.Value<string>("lastName");
            var itemUrl = item.Value<string>("url");


            string imagePathNode;

            try
            {
                imagePathNode = item["cover"].Value<string>("imageUrl");
            }
            catch
            {
                imagePathNode = "ms-appx:///Assets/grey.png";
            }

            var dateNode = item["data"];

            var date = dateNode.Value<string>("date") + " " + dateNode.Value<string>("time");

            var viewsCount = item.Value<string>("totalViews");
            var answerCount = item.Value<string>("totalAnswers");

            var topics = GetTopicsFromSearchResult(item);

            var likes = "+" + item.Value<string>("totalLikes");

            var questionResult = new QuestionModel()
            {
                Id = itemUrl.Split('/')[2],
                Title = title,
                Username = userName,
                Url = string.Format("https://thequestion.ru{0}", itemUrl),
                Topics = topics,
                ImagePath = imagePathNode,
                Date = date.TranslateMonth(),
                ViewsCount = viewsCount,
                AnswersCount = answerCount,
                Likes = likes
            };


            return questionResult;
        }


        private List<Model.Topic> GetTopicsFromSearchResult(JToken node)
        {
            var topicsList = new List<Model.Topic>();

            foreach (var topic in node["topics"])
            {
                var title = topic.Value<string>("title");
                var url = topic.Value<string>("url");
                var id = topic.Value<int>("id");

                topicsList.Add(new Model.Topic()
                {
                    Title = title,
                    Url = url,
                    Id = id
                });

            }

            return topicsList;
        }
    }
}


//<div class=\ "list-questions-ratings__item\"><a href=\ "/account/78820/yuliya-vasileva\" class=\ "list-questions-ratings__item-author\">Юлия Васильева</a><a href=\ "/questions/207801/est-li-khoroshii-sait-dlya-zhelayushikh-nauchitsya-risovat-samostoyatelno\" class=\
//  "list-questions-ratings__item-title\">Есть ли хороший сайт для желающих научиться рисовать самостоятельно?</a>
//  <div class=\ "list-questions-ratings__item-controls\">
//    <div question-like=\ "question-like\" data=\ ":: {&quot;id&quot;:207801,&quot;liked&quot;:false,&quot;totalLikes&quot;:39,&quot;account&quot;:false}\" class=\ "list-questions-ratings__item-controls-icon font-icon theq_like\"></div>
//    <div class=\ "list-questions-ratings__item-controls-counter\"><span>+<span class=\"like-counter\">39</span></span>
//    </div>
//    <div class=\ "list-questions-ratings__item-controls-icon font-icon theq_answers\"></div>
//    <div class=\ "list-questions-ratings__item-controls-counter\">2</div>
//  </div>
//</div>","
//<div class=\ "list-questions-ratings__item\"><a href=\ "/account/57166/anton-chigur\" class=\ "list-questions-ratings__item-author\">Антон Чигур</a><a href=\
//  "/questions/149925/voprosy-grazhdanam-ukrainy-chto-vy-lichno-delaete-dlya-razvitiya-ukrainy-schitaete-li-razvitie-ukrainy-personalno-svoei-zadachei-chitaete-li-knigi-o-razvitii-stran\" class=\ "list-questions-ratings__item-title\">Вопросы гражданам Украины: Что вы лично делаете для развития Украины? Считаете ли развитие Украины персонально своей задачей? Читаете ли книги о развитии стран?</a>
//  <div class=\
//  "list-questions-ratings__item-controls\">
//    <div question-like=\ "question-like\" data=\ ":: {&quot;id&quot;:149925,&quot;liked&quot;:false,&quot;totalLikes&quot;:13,&quot;account&quot;:false}\" class=\ "list-questions-ratings__item-controls-icon font-icon theq_like\"></div>
//    <div class=\ "list-questions-ratings__item-controls-counter\"><span>+<span class=\"like-counter\">13</span></span>
//    </div>
//    <div class=\ "list-questions-ratings__item-controls-icon font-icon theq_answers\"></div>
//    <div class=\ "list-questions-ratings__item-controls-counter\">6</div>
//  </div>
//</div>","