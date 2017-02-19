namespace TheQuestionReborn.API.ContentSources
{
    public abstract class SourceBase
    {
        public ContentFactory Factory { get; set; }

        protected SourceBase()
        {
            Factory = new ContentFactory();
        }
    }
}
