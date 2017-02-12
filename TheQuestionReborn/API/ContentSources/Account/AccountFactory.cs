using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TheQuestionReborn.Model;


namespace TheQuestionReborn.API.ContentSources.Account
{
    public class AccountFactory 
    {
        private readonly HttpClient http;

        public AccountFactory()
        {
            http = new HttpClient();
        }
        public async Task<AccountModel> GetAccountInfo(AccountModel acc)
        {
            var url = "https://thequestion.ru/search/accounts?limit=1&offset=0&q=" + acc.UserName + "&sort=followers";
            var response = await http.GetStringAsync(url);
            var results = JObject.Parse(response);

            var item = results["items"][0];


            string cover;

            try
            {
                cover = item["profile"]["cover"].Value<string>("imageUrl");
            }
            catch
            {
                cover = "ms-appx:///Assets/Profile/profile_default.jpeg";
            }


            acc.Id = item.Value<string>("id");
            acc.HomeTown = item.Value<string>("hometown");
            acc.WebSite = item.Value<string>("website");
            acc.Rating = item.Value<string>("rating");
            acc.TotalAnswers = item.Value<string>("totalAnswers");
            acc.TotalQuestions = item.Value<string>("totalQuestions");
            acc.Cover = cover;


            return acc;
        }
    }
}