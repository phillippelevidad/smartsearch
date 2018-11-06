using SmartSearch;
using SmartSearch.LuceneNet;
using System;

namespace ConsoleApp
{
    static class Searcher
    {
        public static void SearchIndex()
        {
            var domain = Configuration.SearchDomain;
            var options = Configuration.LuceneIndexOptions;
            var searchService = new LuceneSearchService(options);

            var result = searchService.Search(domain, new SearchRequest
            {
                Query = "nike",
                StartIndex = 0,
                PageSize = 100
            });

            foreach (var d in result.Documents)
            {
                foreach (var f in d.Fields)
                    Console.WriteLine($"{f.Key}\t\t{f.Value}");

                Console.WriteLine();
            }
        }
    }
}
