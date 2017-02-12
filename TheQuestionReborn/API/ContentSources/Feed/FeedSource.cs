using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Core;
using TheQuestionReborn.Model;

namespace TheQuestionReborn.API.ContentSources.Feed
{
    public class FeedSource: SourceBase, IIncrementalSource<QuestionModel>
    {
        public async Task<IEnumerable<QuestionModel>> GetPagedItems(int pageSize, int pageIndex)
        {
        
            await ApplicationData.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ApplicationData.IsDataFeedLoadingAnimation.Begin();
            });

            await Factory.GetFeedData(pageSize, pageIndex);

            await ApplicationData.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ApplicationData.IsDataFeedLoadingAnimation.Stop();
            });

            return await Task.Run(() =>
            {
                var result = (from c in ApplicationData.Feed
                              select c).Skip(pageIndex * pageSize).Take(pageSize);

                return result;
            });

          

        }
    }
}
