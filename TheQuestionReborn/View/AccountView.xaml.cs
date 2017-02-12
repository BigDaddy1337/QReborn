using TheQuestionReborn.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using TheQuestionReborn.API;
using TheQuestionReborn.API.ContentSources.Account;
using TheQuestionReborn.Model;

// Шаблон элемента пустой страницы задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234238

namespace TheQuestionReborn.View
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class AccountView : Page
    {
        public AccountView()
        {
            this.InitializeComponent();
            this.DataContext = new AccountViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ApplicationData.AccountAnsweredFeedLoadingCollection = new IncrementalLoadingCollection<AnsweredFeedSource, QuestionModel>(10);
            ApplicationData.AccountQuestionedFeedLoadingCollection = new IncrementalLoadingCollection<QuestionedFeedSource, QuestionModel>(10);
        }
    }


}
