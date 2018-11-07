using SmartSearch;
using SmartSearch.LuceneNet;
using System;

namespace ConsoleApp
{
    static class Searcher
    {
        public static void SearchIndex()
        {
            while (true)
            {
                Console.Write("Query: ");
                var query = Console.ReadLine();

                if (query.Length == 0)
                    break;

                DoSearchAndOutputResults(query);
            }
        }

        static void DoSearchAndOutputResults(string query)
        {
            var domain = new PromonetSearchDomain();
            var options = Configuration.GetLuceneIndexOptions();
            var searchService = new LuceneSearchService(options);

            var result = searchService.Search(domain, new SearchRequest
            {
                Query = query,
                StartIndex = 0,
                PageSize = 4
            });

            Console.WriteLine();
            Console.WriteLine($"{result.TotalCount} resultados");
            Console.WriteLine();

            foreach (var d in result.Documents)
            {
                PrintFieldAndValue("DocId", d.Id);

                foreach (var f in d.Fields)
                    PrintFieldAndValue(f.Key, f.Value);

                Console.WriteLine();
                Console.WriteLine("==============================");
                Console.WriteLine();
            }
        }

        static void PrintFieldAndValue(string field, object value)
        {
            var colSize = 35;
            var spaceCount = colSize - field.Length;
            var spaces = new string(' ', spaceCount);
            Console.WriteLine($"{field}{spaces}{value}");
        }
    }
}
