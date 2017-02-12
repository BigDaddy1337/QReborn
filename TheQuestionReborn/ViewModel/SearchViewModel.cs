using System;
using TheQuestionReborn.API;
using TheQuestionReborn.API.ContentSources;
using TheQuestionReborn.API.ContentSources.Search;
using TheQuestionReborn.Model;
using TheQuestionReborn.MVVMBase;
using TheQuestionReborn.View;

namespace TheQuestionReborn.ViewModel
{
    public class SearchViewModel : ViewModelBase
    {
        public IncrementalLoadingCollection<SearchSource, QuestionModel> SearchFeed => ApplicationData.SearchTopicFeedLoadingCollection;

        public DelegateCommand<string> TextChangedCommand => new DelegateCommand<string>(TextChanged);

        public DelegateCommand<QuestionModel> ItemClickCommand => new DelegateCommand<QuestionModel>(ItemClick);

        private void ItemClick(QuestionModel clickedItem)
        {
            ApplicationData.Question = clickedItem;
            ApplicationData.AppFrame.Navigate(typeof(QuestionView));
        }

        private void TextChanged(string text)
        {
            if (text == " " || text == "") return;

            ApplicationData.SearchKey = text;
            ApplicationData.SearchTopicFeedLoadingCollection.RefreshSearchItems();
        }

    }
}
