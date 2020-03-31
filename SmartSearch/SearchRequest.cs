using SmartSearch.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmartSearch
{
    public class SearchRequest : ISearchRequest
    {
        public const int DefaultPageSize = 100;

        public SearchRequest(string query = null, int startIndex = 0, int pageSize = DefaultPageSize, IEnumerable<IFilter> filters = null, IEnumerable<ISortOption> sortOptions = null)
        {
            Query = query;
            StartIndex = startIndex;
            PageSize = pageSize;
            Filters = (filters ?? Array.Empty<IFilter>()).ToList().AsReadOnly();
            SortOptions = (sortOptions ?? Array.Empty<ISortOption>()).ToList().AsReadOnly();
        }

        public string Query { get; }
        public int StartIndex { get; }
        public int PageSize { get; } = DefaultPageSize;
        public ReadOnlyCollection<IFilter> Filters { get; }
        public ReadOnlyCollection<ISortOption> SortOptions { get; }
    }

    public class SearchRequestBuilder
    {
        private string query;
        private int startIndex;
        private int pageSize;
        private readonly List<IFilter> filters = new List<IFilter>();
        private readonly List<ISortOption> sortOptions = new List<ISortOption>();

        public SearchRequestBuilder(string query = null, int startIndex = 0, int pageSize = SearchRequest.DefaultPageSize)
        {
            this.query = query;
            this.startIndex = startIndex;
            this.pageSize = pageSize;
        }

        public SearchRequestBuilder Query(string query)
        {
            this.query = query;
            return this;
        }

        public SearchRequestBuilder StartIndex(int startIndex)
        {
            this.startIndex = startIndex;
            return this;
        }

        public SearchRequestBuilder PageSize(int pageSize)
        {
            this.pageSize = pageSize;
            return this;
        }

        public SearchRequestBuilder FilterBy(IFilter filter)
        {
            filters.Add(filter);
            return this;
        }
        public SearchRequestBuilder FilterBy(string fieldName, object value) => FilterBy(new ValueFilter(fieldName, value));
        public SearchRequestBuilder FilterBy(string fieldName, object fromValue, object toValue) => FilterBy(new RangeFilter(fieldName, fromValue, toValue));
        public SearchRequestBuilder FilterBy(IEnumerable<IFilter> filters)
        {
            this.filters.AddRange(filters);
            return this;
        }

        public SearchRequestBuilder SortBy(ISortOption sortOption)
        {
            sortOptions.Add(sortOption);
            return this;
        }
        public SearchRequestBuilder SortBy(string fieldName, SortDirection direction) => SortBy(new SortOption(fieldName, direction));
        public SearchRequestBuilder SortBy(string fieldName, SortDirection direction, ILatLng origin) => SortBy(new LatLngSortOption(fieldName, direction, origin));
        public SearchRequestBuilder SortBy(IEnumerable<ISortOption> sortOptions)
        {
            this.sortOptions.AddRange(sortOptions);
            return this;
        }

        public SearchRequest Build() => new SearchRequest(query, startIndex, pageSize, filters, sortOptions);
    }
}