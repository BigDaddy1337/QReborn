using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TheQuestionReborn.Helpers;
using TheQuestionReborn.Model;

namespace TheQuestionReborn.API.ContentSources.Search
{
    public class SearchSource : SourceBase, IIncrementalSource<QuestionModel>
    {
        public async Task<IEnumerable<QuestionModel>> GetPagedItems(int pageSize, int pageIndex)
        {
            await GetSearchContent(ApplicationData.SearchKey, pageSize, pageIndex);

            return await Task.Run(() =>
            {
                var result = (from c in ApplicationData.SearchTopicFeed
                              select c).Skip(pageIndex * pageSize).Take(pageSize);

                return result;
            });

        }
        public async Task GetSearchContent(string keyword, int limit, int pageIndex)
        {
            var url = "https://thequestion.ru/search/questions?limit=" + limit + "&offset=" + limit * pageIndex + "&q=" + keyword + "&sort=followers";
            var http = new HttpClient();

            http.DefaultRequestHeaders.Referrer = new Uri("http://thequestion.ru/all?qsort=answer");
            http.DefaultRequestHeaders.Host = "thequestion.ru";

            var response = await http.GetStringAsync(url);
            var results = JObject.Parse(response);

            foreach (var item in results["items"])
            {
                var imagePath = string.Empty;
                var title = item.Value<string>("title");
                var account = item["account"];
                var userName = account.Value<string>("firstName") + " " + account.Value<string>("lastName");
                var itemUrl = item.Value<string>("url");


                var imagePathNode = string.Empty;

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

                var questionResult = new QuestionModel()
                {
                    Id = itemUrl.Split('/')[2],
                    Title = title,
                    Username = userName,
                    Url = string.Format("https://thequestion.ru{0}", itemUrl),
                    Topics = topics,
                    ImagePath = imagePath,
                    Date = date.TranslateMonth(),
                    ViewsCount = viewsCount,
                    AnswersCount = answerCount
                };

                ApplicationData.SearchTopicFeed.Add(questionResult);
            }
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
