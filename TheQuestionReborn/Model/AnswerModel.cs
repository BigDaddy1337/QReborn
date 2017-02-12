using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheQuestionReborn.Model
{
    public class AnswerModel
    {
        public string Id { get; set; }
        public AccountModel User { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }
        public string Votes { get; set; }
        public string Comments { get; set; }
    }
}
