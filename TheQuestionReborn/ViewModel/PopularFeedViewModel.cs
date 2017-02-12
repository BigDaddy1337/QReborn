using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheQuestionReborn.API;
using TheQuestionReborn.API.ContentSources;
using TheQuestionReborn.Helpers;
using TheQuestionReborn.Model;
using TheQuestionReborn.MVVMBase;
using TheQuestionReborn.View;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Media;
using TheQuestionReborn.API.ContentSources.Feed;

namespace TheQuestionReborn.ViewModel
{
    public class FeedMenu
    {
        public string Title { get; set; }
        public string Color { get; set; }
    }

    public class PopularFeedViewModel : ViewModelBase
    {
        private List<FeedMenu> menu = new List<FeedMenu>();

        private SolidColorBrush day = new SolidColorBrush(Colors.DarkGray);
        private SolidColorBrush week = new SolidColorBrush(Colors.White);
        private SolidColorBrush month = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush white = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush darkGay = new SolidColorBrush(Colors.DarkGray);

        public PopularFeedViewModel()
        {
            InitializeMenu();
        }
        
        private void InitializeMenu()
        {
            Menu.Add(new FeedMenu { Title = "День"});
            Menu.Add(new FeedMenu { Title = "Неделя"});
            Menu.Add(new FeedMenu { Title = "Месяц"});
        }

        public SolidColorBrush Day
        {
            get
            {
                return day;
            }
            set
            {
                if (value != white)
                {
                    Week = white;
                    Month = white;
                }

                day = value;
                RaisePropertyChanged("Day");
            }
        }

        public SolidColorBrush Week
        {
            get
            {
                return week;
            }
            set
            {
                if (value != white)
                {
                    Day = white;
                    Month = white;
                }
                week = value;
                RaisePropertyChanged("Week");
            }
        }
        public SolidColorBrush Month
        {
            get
            {
                return month;
            }
            set
            {
                if (value != white)
                {
                    Week = white;
                    Day = white;
                }
                month = value;
                RaisePropertyChanged("Month");
            }
        }


        public List<FeedMenu> Menu
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

        public IncrementalLoadingCollection<PopularFeedSource, QuestionModel> PopularFeed => ApplicationData.PopularFeedLoadingCollection;

        public DelegateCommand<string> CategoryClickCommand => new DelegateCommand<string>(CategoryClick);

        public DelegateCommand<object> RefreshButtonClickCommand => new DelegateCommand<object>(RefreshButtonClick);

        public DelegateCommand<QuestionModel> ItemClickCommand => new DelegateCommand<QuestionModel>(ItemClick);

        private void CategoryClick(string category)
        {
            switch (category)
            {
                case "День":
                    ApplicationData.PopularFeedPeriod = PopularFeedPeriod.day;
                    Day = darkGay;
                    ApplicationData.PopularFeedLoadingCollection.RefreshPopularFeedItems();
                    break;
                case "Неделя":
                    ApplicationData.PopularFeedPeriod = PopularFeedPeriod.week;
                    Week = darkGay;
                    ApplicationData.PopularFeedLoadingCollection.RefreshPopularFeedItems();
                    break;
                case "Месяц":
                    ApplicationData.PopularFeedPeriod = PopularFeedPeriod.month;
                    Month = darkGay;
                    ApplicationData.PopularFeedLoadingCollection.RefreshPopularFeedItems();
                    break;
            }
            
        }

        private void RefreshButtonClick(object parameter)
        {
            ApplicationData.PopularFeedLoadingCollection.RefreshPopularFeedItems();
        }

        private void ItemClick(QuestionModel clickedItem)
        {
            ApplicationData.Question = clickedItem;
            ApplicationData.AppFrame.Navigate(typeof(QuestionView));
        }


    }
}
