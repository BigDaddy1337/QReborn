using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace TheQuestionReborn.Model
{
    public class QuestionModel
    {
        private string imagePath ;


        public string Id { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public string Url { get; set; }
        public string ViewsCount { get; set; }
        public List<Topic> Topics { get; set; }
        public string AnswersCount { get; set; }
        public List<AnswerModel> Answers { get; set; }
        public string Date { get; set; }
        public string Likes { get; set; }
        
        public string PreviewAnswer { get; set; }
        public AccountModel AccountAnswer { get; set; }

        public string ImagePath
        {
            get
            {
                return imagePath;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    ImageBackground = "ms-appx:///Assets/grey.png";
                }

                imagePath = value;
            }
        }

        public string ImageBackground { get; private set; } = "ms-appx:///Assets/black.png";
    }

    //public class Title
    //{
    //    private Brush color;
    //    public string Text { get; set; }

    //    public Brush Color
    //    {
    //        get
    //        {
    //            return color;
    //        }

    //        set
    //        {
    //            color = value;
    //        }
    //    }
    //}


    public class Topic
    {
        public Brush Color { get; set; }

        public string Title { get; set; }
        public string Url { get; set; }
        public string CountOfQuestions { get; set; }
        public int Id { get; set; }
        

        public string ColorStr
        {
            set
            {
                Color = ColorToBrush(value);
            }
        }

        private static Brush ColorToBrush(string color) // color = "#E7E44D"
        {
            try
            {
                if (color.Length == 6)
                {
                    return new SolidColorBrush(ColorHelper.FromArgb(255,
                        byte.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                        byte.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                        byte.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber)));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return new SolidColorBrush(Colors.DimGray);
            }

        }

    }
}
