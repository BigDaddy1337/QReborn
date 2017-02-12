using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheQuestionReborn.Model;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Animation;

namespace TheQuestionReborn.API
{
    public interface IIncrementalSource<T>
    {
        Task<IEnumerable<T>> GetPagedItems(int pageSize, int pageIndex);
    }

    public class IncrementalLoadingCollection<T, I> : ObservableCollection<I>,
         ISupportIncrementalLoading
         where T : IIncrementalSource<I>, new()
    {
        private readonly T source;
        private readonly int itemsPerPage;
        private int currentPage;

        public IncrementalLoadingCollection(int itemsPerPage)
        {
            this.source = new T();
            this.itemsPerPage = itemsPerPage;
            this.HasMoreItems = true;
        }

        public bool HasMoreItems { get; set; }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            var dispatcher = Window.Current.Dispatcher;
            var resultCount = 0;

            return Task.Run(async () =>
            {
                var result = await source.GetPagedItems(itemsPerPage, currentPage++);

                if (result == null || result.Count() == 0)
                {
                    HasMoreItems = false;
                }
                else
                {
                    resultCount = result.Count();
                    await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                     {
                         foreach (var item in result.ToList())
                             this.Add(item);

                     });
                }
                return new LoadMoreItemsResult() { Count = (uint)resultCount };

            }).AsAsyncOperation();
        }

        public void RefreshFeedItems()
        {
            currentPage = 0;
            ApplicationData.Feed.Clear();
            this.ClearItems();
            HasMoreItems = true;
        }

        public void RefreshPopularFeedItems()
        {
            currentPage = 0;
            ApplicationData.PopularFeed.Clear();
            this.ClearItems();
            HasMoreItems = true;
        }

        public void RefreshTopicItems()
        {
            currentPage = 0;
            ApplicationData.TopicFeed.Clear();
            this.ClearItems();
            HasMoreItems = true;
        }

        public async void RefreshSearchItems()
        {
            currentPage = 0;
            ApplicationData.SearchTopicFeed.Clear();
            this.ClearItems();
            HasMoreItems = true;
            await LoadMoreItemsAsync(10);
        }
    }
}
