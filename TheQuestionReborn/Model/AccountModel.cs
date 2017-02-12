using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheQuestionReborn.Model
{
    public class AccountModel
    {
        public string Id { get; set; }
        public string HomeTown { get; set; }
        public string WebSite { get; set; }
        public string TotalAnswers { get; set; }
        public string TotalQuestions { get; set; }
        public string Cover { get; set; }

        public string UserName { get; set; }
        public string Url { get; set; }
        public string ImagePath { get; set; }
        public string Rating { get; set; }
        public string Info { get; set; }
    }
}
