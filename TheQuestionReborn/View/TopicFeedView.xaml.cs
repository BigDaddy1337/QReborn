using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TheQuestionReborn.API;
using TheQuestionReborn.API.ContentSources;
using TheQuestionReborn.Model;
using TheQuestionReborn.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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
