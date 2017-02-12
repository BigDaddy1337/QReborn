using TheQuestionReborn.Model;
using TheQuestionReborn.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// Шаблон элемента пустой страницы задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234238

namespace TheQuestionReborn.View
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class QuestionView : Page
    {
        private readonly QuestionViewModel questionViewModel;

        public QuestionView()
        {
            this.InitializeComponent();
            questionViewModel = new QuestionViewModel();
            DataContext = questionViewModel;
            //this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private void Scroll_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            TopPanel.Opacity = (-Scroll.VerticalOffset + 100) / 20;
            Bar.Opacity = (Scroll.VerticalOffset - 120) / 20;
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var control = sender as Grid;
            var item = control?.DataContext as Topic;
            ApplicationData.Topic = item;
            ApplicationData.AppFrame.Navigate(typeof(TopicFeedView));

        }

        private void CommentsTap(object sender, TappedRoutedEventArgs e)
        {
            var panel = sender as StackPanel;

            var answer = panel?.DataContext as AnswerModel;

            if (int.Parse(answer?.Comments) == 0)
                return;

            CommentsAnimationAppearance.Begin();

            CommentsGrid.Visibility = Visibility.Visible;

            questionViewModel.GetComments(answer);

        }

        private void CommentManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (e.Cumulative.Translation.X > 20)
            {
                CommentsAnimationHide.Begin();
            }
        }

        private void UserGridTapped(object sender, TappedRoutedEventArgs e)
        {
            var control = sender as Grid;
            var item = control?.DataContext as AnswerModel;
            ApplicationData.Account = item?.User;
            ApplicationData.AppFrame.Navigate(typeof(AccountView));
        }
    }
}
