using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheQuestionReborn.Model
{
    public class CommentModel
    {
        public AccountModel Author { get; set; }
        public string Date { get; set; }
        public string Text { get; set; }
        public string Votes { get; set; }
    }
}
