using System;
using System.Collections.Generic;
using TheQuestionReborn.API;
using TheQuestionReborn.Model;
using TheQuestionReborn.MVVMBase;
using Windows.UI.Core;
using Windows.UI.Xaml;

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
        private string error = string.Empty;

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




        public string Error
        {
            get
            {
                return error;
            }
            set
            {
                error = value;
                RaisePropertyChanged("Error");
            }

        }

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

                try
                {
                    if (question.Answers == null)
                        ListAnswers = await factory.GetQuestionBody(question);
                    else
                        ListAnswers = question.Answers;

                }
                catch (Exception e)
                {
                    Error = "Ошибка загрузки страницы";
                }
                finally
                {
                    ProgressGridVisibility = Visibility.Collapsed;
                }

            });
        }
    }
}
