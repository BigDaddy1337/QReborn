using TheQuestionReborn.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Шаблон элемента пустой страницы задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234238

namespace TheQuestionReborn.View
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class TopicsView : Page
    {
        public TopicsView()
        {
            this.InitializeComponent();
            var viewModel = new TopicsViewModel();
            DataContext = viewModel;
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }
    }
}
