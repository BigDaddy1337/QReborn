using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheQuestionReborn.Model;

namespace TheQuestionReborn.API.ContentSources.Topic
{
    public class TopicFeedSource : SourceBase, IIncrementalSource<QuestionModel>
    {
        public async Task<IEnumerable<QuestionModel>> GetPagedItems(int pageSize, int pageIndex)
        {
            await Factory.GetTopicFeedData(pageSize, pageIndex);

            return await Task.Run(() =>
            {
                var result = (from c in ApplicationData.TopicFeed
                              select c).Skip(pageIndex * pageSize).Take(pageSize);

                return result;
            });
        }
    }
}
