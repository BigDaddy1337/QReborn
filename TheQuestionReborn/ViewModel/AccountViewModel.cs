using System;
using TheQuestionReborn.API.ContentSources.Account;
using TheQuestionReborn.Model;
using TheQuestionReborn.MVVMBase;
using Windows.UI.Core;
using Windows.UI.Xaml;
using TheQuestionReborn.API;
using TheQuestionReborn.API.ContentSources.Feed;
using TheQuestionReborn.View;

namespace TheQuestionReborn.ViewModel
{
    public class AccountViewModel : ViewModelBase
    {
        private readonly AccountFactory accountFactory;
        private AccountModel account = new AccountModel();

        public AccountViewModel()
        {
            accountFactory = new AccountFactory();
            GetInfo(ApplicationData.Account);
        }

        public AccountModel Account
        {
            get
            {
                return account;
            }
            set
            {
                account = value;
                RaisePropertyChanged("Account");
            }
        }

        public IncrementalLoadingCollection<AnsweredFeedSource, QuestionModel> AnsweredFeed => ApplicationData.AccountAnsweredFeedLoadingCollection;
        public IncrementalLoadingCollection<QuestionedFeedSource, QuestionModel> QuestenedFeed => ApplicationData.AccountQuestionedFeedLoadingCollection;
        
        private async void GetInfo(AccountModel acc)
        {
            var dispatcher = Window.Current.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {

                //ProgressGridVisibility = Visibility.Visible;

                Account = await accountFactory.GetAccountInfo(acc);

               // ProgressGridVisibility = Visibility.Collapsed;
            });
        }

        public DelegateCommand<QuestionModel> ItemClickCommand => new DelegateCommand<QuestionModel>(ItemClick);

        private void ItemClick(QuestionModel clickedItem)
        {
            ApplicationData.Question = clickedItem;
            ApplicationData.AppFrame.Navigate(typeof(QuestionView));
        }
    }
}
