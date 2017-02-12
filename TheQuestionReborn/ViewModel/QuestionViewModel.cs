using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheQuestionReborn.API;
using TheQuestionReborn.Model;
using TheQuestionReborn.MVVMBase;
using TheQuestionReborn.View;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace TheQuestionReborn.ViewModel
{
    public class QuestionViewModel : ViewModelBase
    {
        private readonly ContentFactory factory;
        private QuestionModel question;
        private List<AnswerModel> answers;
        private List<CommentModel> comments;
        private Visibility progressGridVisibility;
        private Visibility progressCommentsVisibility;

        public QuestionViewModel()
        {
            ApplicationData.MainVm.CurrentPageName = string.Empty;
            Question = ApplicationData.Question;
            factory = new ContentFactory();
            GetAnswers(Question);
        }

        public QuestionModel Question
        {
            get
            {
                return question;
            }
            set
            {
                question = value;
                RaisePropertyChanged("Question");
            }

        }


        public List<AnswerModel> ListAnswers
        {
            get
            {
                return answers;
            }
            set
            {
                answers = value;
                RaisePropertyChanged("ListAnswers");
            }

        }

        public List<CommentModel> ListComments
        {
            get
            {
                return comments;
            }
            set
            {
                comments = value;
                RaisePropertyChanged("ListComments");
            }

        }

        //public bool ProgressRingIsActive
        //{
        //    get
        //    {
        //        return progressBarIsActive;
        //    }

        //    set
        //    {
        //        progressBarIsActive = value;
        //        RaisePropertyChanged("ProgressBarIsActive");
        //    }
        //}

        public Visibility ProgressGridVisibility
        {
            get
            {
                return progressGridVisibility;
            }

            set
            {
                progressGridVisibility = value;
                RaisePropertyChanged("ProgressGridVisibility");
            }
        }

        public Visibility ProgressCommentsVisibility
        {
            get
            {
                return progressCommentsVisibility;
            }

            set
            {
                progressCommentsVisibility = value;
                RaisePropertyChanged("ProgressCommentsVisibility");
            }
        }



        //public DelegateCommand<Topic> ItemClickCommand
        //{
        //    get { return new DelegateCommand<Topic>(ItemClick); }
        //}

        //private void ItemClick(Topic clickedItem)
        //{
        //    ApplicationData.Topic = clickedItem;
        //    ApplicationData.AppFrame.Navigate(typeof(TopicFeedView));
        //}

        public async void GetComments(AnswerModel answer)
        {
            var dispatcher = Window.Current.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {

                ProgressCommentsVisibility = Visibility.Visible;

                ListComments = await factory.GetCommentsFromAnswer(ApplicationData.Question.Id, answer.Id);

                ProgressCommentsVisibility = Visibility.Collapsed;
            });
        }

        private async void GetAnswers(QuestionModel question)
        {
            var dispatcher = Window.Current.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {

                    ProgressGridVisibility = Visibility.Visible;

                    if (question.Answers == null)
                        ListAnswers = await factory.GetQuestionBody(question);
                    else
                        ListAnswers = question.Answers;

                    ProgressGridVisibility = Visibility.Collapsed;
            });
        }
    }
}
