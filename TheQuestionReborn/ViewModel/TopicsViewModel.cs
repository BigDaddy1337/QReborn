using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheQuestionReborn.API;
using TheQuestionReborn.API.ContentSources;
using TheQuestionReborn.API.ContentSources.Topic;
using TheQuestionReborn.Model;
using TheQuestionReborn.MVVMBase;
using TheQuestionReborn.View;

namespace TheQuestionReborn.ViewModel
{
    public class TopicsViewModel : ViewModelBase
    {
        public IncrementalLoadingCollection<TopicSource, Topic> TopicsFeed => ApplicationData.TopicsLoadingCollection;

        public DelegateCommand<Topic> ItemClickCommand => new DelegateCommand<Topic>(ItemClick);

        private void ItemClick(Topic clickedItem)
        {
            ApplicationData.Topic = clickedItem;
            ApplicationData.AppFrame.Navigate(typeof(TopicFeedView));
        }
    }
}
