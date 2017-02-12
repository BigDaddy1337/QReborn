using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheQuestionReborn.API;
using TheQuestionReborn.API.ContentSources;
using TheQuestionReborn.Helpers;
using TheQuestionReborn.View;
using TheQuestionReborn.ViewModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using TheQuestionReborn.API.ContentSources.Account;
using TheQuestionReborn.API.ContentSources.Feed;
using TheQuestionReborn.API.ContentSources.Search;
using TheQuestionReborn.API.ContentSources.Topic;

namespace TheQuestionReborn.Model
{
    public static class ApplicationData
    {
        public static CoreDispatcher Dispatcher = Window.Current.Dispatcher;

        /// <summary>
        /// Главная ViewModel
        /// </summary>
        public static MainViewModel MainVm { get; set; }

        /// <summary>
        /// Фрейм в котором перемещаемся
        /// </summary>
        public static Frame AppFrame = new Frame();

        /// <summary>
        /// Текущий вопрос
        /// </summary>
        public static QuestionModel Question = new QuestionModel();

        /// <summary>
        /// Просмотр профиля пользователя
        /// </summary>
        public static AccountModel Account; 
        
        /// <summary>
        /// Лента вопросов с ответами пользователя
        /// </summary>
        public static IncrementalLoadingCollection<AnsweredFeedSource, QuestionModel> AccountAnsweredFeedLoadingCollection = new IncrementalLoadingCollection<AnsweredFeedSource, QuestionModel>(10);
        public static IncrementalLoadingCollection<QuestionedFeedSource, QuestionModel> AccountQuestionedFeedLoadingCollection = new IncrementalLoadingCollection<QuestionedFeedSource, QuestionModel>(10);

        /// <summary>
        /// Лента вопросов
        /// </summary>
        public static ObservableCollection<QuestionModel> Feed = new ObservableCollection<QuestionModel>();
        public static IncrementalLoadingCollection<FeedSource, QuestionModel> FeedLoadingCollection = new IncrementalLoadingCollection<FeedSource, QuestionModel>(10);

        /// <summary>
        /// Лента популярного
        /// </summary>
        public static ObservableCollection<QuestionModel> PopularFeed = new ObservableCollection<QuestionModel>();
        public static IncrementalLoadingCollection<PopularFeedSource, QuestionModel> PopularFeedLoadingCollection = new IncrementalLoadingCollection<PopularFeedSource, QuestionModel>(10);
        public static PopularFeedPeriod PopularFeedPeriod = PopularFeedPeriod.day;
        /// <summary>
        /// Анимация загрузки ленты вопросов
        /// </summary>
        public static Storyboard IsDataFeedLoadingAnimation = new Storyboard();

        /// <summary>
        /// Анимация загрузки ленты популярного
        /// </summary>
        public static Storyboard IsDataFeedPopularLoadingAnimation = new Storyboard();

        /// <summary>
        /// Лента тем
        /// </summary>
        public static ObservableCollection<Topic> Topics = new ObservableCollection<Topic>();
        public static IncrementalLoadingCollection<TopicSource, Topic> TopicsLoadingCollection = new IncrementalLoadingCollection<TopicSource, Topic>(24);
        
        /// <summary>
        /// Текущая тема
        /// </summary>
        public static Topic Topic = new Topic();

        /// <summary>
        /// Лента вопросов в теме
        /// </summary>
        public static ObservableCollection<QuestionModel> TopicFeed = new ObservableCollection<QuestionModel>();
        public static IncrementalLoadingCollection<TopicFeedSource, QuestionModel> TopicFeedLoadingCollection = new IncrementalLoadingCollection<TopicFeedSource, QuestionModel>(10);

        /// <summary>
        /// Лента поиска
        /// </summary>
        public static string SearchKey = string.Empty;
        public static ObservableCollection<QuestionModel> SearchTopicFeed = new ObservableCollection<QuestionModel>();
        public static IncrementalLoadingCollection<SearchSource, QuestionModel> SearchTopicFeedLoadingCollection = new IncrementalLoadingCollection<SearchSource, QuestionModel>(10);
    }
}
