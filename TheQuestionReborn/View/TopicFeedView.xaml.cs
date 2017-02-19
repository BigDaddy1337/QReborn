using TheQuestionReborn.Model;
using TheQuestionReborn.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Шаблон элемента пустой страницы задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234238

namespace TheQuestionReborn.View
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class TopicFeedView : Page
    {
        private readonly TopicFeedViewModel vm;

        public TopicFeedView()
        {
            this.InitializeComponent();
            vm = new TopicFeedViewModel();
            DataContext = vm;
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ApplicationData.MainVm.CurrentPageName = ApplicationData.Topic.Title;

            if (vm.CurrentTopic == ApplicationData.Topic) return;

            ApplicationData.TopicFeedLoadingCollection.RefreshTopicItems();
            vm.CurrentTopic = ApplicationData.Topic;
        }

    }
}
