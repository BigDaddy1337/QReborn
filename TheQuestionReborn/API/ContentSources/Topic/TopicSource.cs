using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheQuestionReborn.Model;

namespace TheQuestionReborn.API.ContentSources.Topic
{
    public class TopicSource : SourceBase, IIncrementalSource<Model.Topic>
    {
        public async Task<IEnumerable<Model.Topic>> GetPagedItems(int pageSize, int pageIndex)
        {
            await Factory.GetTopicsData(pageSize, pageIndex);

            return await Task.Run(() =>
            {
                var result = (from c in ApplicationData.Topics
                              select c).Skip(pageIndex * pageSize).Take(pageSize);

                return result;
            });

        }
    }
}
