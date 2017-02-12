using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TheQuestionReborn.API.ContentSources.Account.Base;
using TheQuestionReborn.Model;

namespace TheQuestionReborn.API.ContentSources.Account
{
    public class AnsweredFeedSource : AccountSourceBase, IIncrementalSource<QuestionModel>
    {
        private ObservableCollection<QuestionModel> questions;
        private const string UrlAnswered = "https://thequestion.ru/lists/questions?answererId=";

        public AnsweredFeedSource()
        {
            questions = new ObservableCollection<QuestionModel>();
        }

        public async Task<IEnumerable<QuestionModel>> GetPagedItems(int pageSize, int pageIndex)
        {
            questions = await GetQuestions(UrlAnswered, ApplicationData.Account, pageSize, pageIndex);

            return questions;
        }
    }
}
