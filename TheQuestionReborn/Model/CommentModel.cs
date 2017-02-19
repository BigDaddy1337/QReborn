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
