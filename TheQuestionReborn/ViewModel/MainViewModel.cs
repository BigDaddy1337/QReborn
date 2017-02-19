using System;
using System.Collections.Generic;
using TheQuestionReborn.Model;
using TheQuestionReborn.MVVMBase;
using TheQuestionReborn.View;
using Windows.UI.Xaml.Navigation;

namespace TheQuestionReborn.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private List<MenuItem> menu;
        private bool isMenuOpen;
        private string currentPageName;
        private MenuItem selectedIndexmenu;

        public Type CurrentPage { get; set; }

        public MainViewModel()
        {
            menu = new List<MenuItem>();
            ApplicationData.AppFrame.Navigating += Navigating;
            InitializeMenu();

        }

        private void Navigating(object sender, NavigatingCancelEventArgs e)
        {
            CurrentPage = e.SourcePageType;

            if (e.SourcePageType == typeof(QuestionView) || e.SourcePageType == typeof(TopicFeedView) || e.SourcePageType == typeof(AccountView)) return;

            var navigatedPage = Menu.Find(x => x.PageType == e.SourcePageType);
            CurrentPageName = navigatedPage.Title;
        }

        private void InitializeMenu()
        {
            Menu.Add(new MenuItem { Icon = "", Title = "Лента", PageType = typeof(FeedView) });
            Menu.Add(new MenuItem { Icon = "", Title = "Популярное", PageType = typeof(PopularFeedView) });
            Menu.Add(new MenuItem { Icon = "", Title = "Темы", PageType = typeof(TopicsView) });
            Menu.Add(new MenuItem { Icon = "", Title = "Поиск", PageType = typeof(SearchView) });
            Menu.Add(new MenuItem { Icon = "", Title = "Настройки", PageType = typeof(SettingsView) });

            //Menu.Add(new MenuItem { Icon = "", Title = "Настройки", PageType = typeof(SettingsPage) });
            //Menu.Add(new MenuItem { Icon = "", Title = "Выйти", PageType = typeof(AuthPage) });

            //QuestionedFeedSource content = new QuestionedFeedSource();
            //await content.GetQuestions(new AccountModel() { Id = "155243" }, 5, 0);

            NavigateNextPage(Menu[0]);

 
        }

        public List<MenuItem> Menu
        {
            get
            {
                return menu;
            }

            set
            {
                menu = value;
                RaisePropertyChanged("Menu");
            }
        }

        public bool IsMenuOpen
        {
            get
            {
                return isMenuOpen;
            }

            set
            {
                isMenuOpen = value;
                RaisePropertyChanged("IsMenuOpen");
            }
        }

        public MenuItem SelectedItemMenu
        {
            get
            {
                return selectedIndexmenu;
            }

            set
            {
                selectedIndexmenu = value;

                NavigateNextPage(value);

                RaisePropertyChanged("SelectedItemMenu");
            }
        }

        public string CurrentPageName
        {
            get
            {
                return currentPageName.ToUpper();
            }
            set
            {
                currentPageName = value;
                RaisePropertyChanged("CurrentPageName");
            }
        }

        public DelegateCommand<object> MenuButtonClickCommand => new DelegateCommand<object>(MenuButtonClick);

        private void MenuButtonClick(object prarmeter)
        {
            IsMenuOpen = true; 
        }

        public DelegateCommand<MenuItem> MenuItemClickCommand => new DelegateCommand<MenuItem>(ItemClick);

        private void ItemClick(MenuItem clickedItem)
        {
            IsMenuOpen = false;
            ApplicationData.AppFrame.Navigate(clickedItem.PageType);
        }

        private void NavigateNextPage(MenuItem item)
        {
            IsMenuOpen = false;
            ApplicationData.AppFrame.Navigate(item.PageType);
        }
    }
}
