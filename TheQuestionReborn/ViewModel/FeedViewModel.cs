using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheQuestionReborn.API;
using TheQuestionReborn.API.ContentSources;
using TheQuestionReborn.API.ContentSources.Feed;
using TheQuestionReborn.Model;
using TheQuestionReborn.MVVMBase;
using TheQuestionReborn.View;

namespace TheQuestionReborn.ViewModel
{
    public class FeedViewModel : ViewModelBase
    {
        public IncrementalLoadingCollection<FeedSource, QuestionModel> Feed => ApplicationData.FeedLoadingCollection;

        public DelegateCommand<object> RefreshButtonClickCommand => new DelegateCommand<object>(RefreshButtonClick);

        public DelegateCommand<QuestionModel> ItemClickCommand => new DelegateCommand<QuestionModel>(ItemClick);

        private void RefreshButtonClick(object parameter)
        {
            ApplicationData.FeedLoadingCollection.RefreshFeedItems();
        }

        private void ItemClick(QuestionModel clickedItem) 
        {
            ApplicationData.Question = clickedItem;
            ApplicationData.AppFrame.Navigate(typeof(QuestionView));
        }

    }
}
