using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using TheQuestionReborn.API.ContentSources.Account.Base;
using TheQuestionReborn.Helpers;
using TheQuestionReborn.Model;

namespace TheQuestionReborn.API.ContentSources.Account
{
    public class QuestionedFeedSource : AccountSourceBase, IIncrementalSource<QuestionModel>
    {
        private ObservableCollection<QuestionModel> questions;

        private const string UrlQuestioned = "https://thequestion.ru/lists/questions?filter=questions&userId=";

        public QuestionedFeedSource()
        {
            questions = new ObservableCollection<QuestionModel>();
        }

        public async Task<IEnumerable<QuestionModel>> GetPagedItems(int pageSize, int pageIndex)
        {
            questions = await GetQuestions(UrlQuestioned, ApplicationData.Account, pageSize, pageIndex);

            return questions;
        }

    }
}
