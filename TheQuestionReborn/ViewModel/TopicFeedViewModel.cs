using TheQuestionReborn.API;
using TheQuestionReborn.API.ContentSources;
using TheQuestionReborn.API.ContentSources.Topic;
using TheQuestionReborn.Model;
using TheQuestionReborn.MVVMBase;
using TheQuestionReborn.View;

namespace TheQuestionReborn.ViewModel
{
    public class TopicFeedViewModel : ViewModelBase
    {
        public TopicFeedViewModel()
        {
            CurrentTopic = new Topic();
        }

        public Topic CurrentTopic { get; set; }

        public IncrementalLoadingCollection<TopicFeedSource, QuestionModel> TopicFeed => ApplicationData.TopicFeedLoadingCollection;

        public DelegateCommand<QuestionModel> ItemClickCommand => new DelegateCommand<QuestionModel>(ItemClick);

        private void ItemClick(QuestionModel clickedItem)
        {
            ApplicationData.Question = clickedItem;
            ApplicationData.AppFrame.Navigate(typeof(QuestionView));
        }
    }
}
