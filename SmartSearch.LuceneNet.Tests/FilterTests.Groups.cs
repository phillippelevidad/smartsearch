using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Tests.Mocks;
using System;

namespace SmartSearch.LuceneNet.Tests
{
    public partial class FilterTests
    {
        [TestMethod]
        public void GroupFiltersWork()
        {
            var env = TestEnvironment.Build();

            TestLocations(env, new FilterGroup(GroupingClause.And,
                new RangeFilter("AddedDate", new DateTime(2018, 1, 1), new DateTime(2018, 3, 1)),
                new RangeFilter("Price", 0, 5000)
            ), "Impact Hub Curitiba", "Parque Barigui", "Ouro Branco MG");

            TestLocations(env, new FilterGroup(GroupingClause.And,
                new RangeFilter("AddedDate", new DateTime(2018, 1, 1), new DateTime(2018, 3, 1)),
                new RangeFilter("Price", 0, 1000)
            ), "Parque Barigui", "Ouro Branco MG");

            TestLocations(env, new FilterGroup(GroupingClause.Or,
                new ValueFilter("Category", "Eletrodomésticos"),
                new ValueFilter("Category", "Eletrônicos")
            ), "Impact Hub Curitiba", "Parque Barigui");

            TestLocations(env, new FilterGroup(GroupingClause.And,
                new ValueFilter("Category", "Eletrodomésticos"),
                new ValueFilter("Category", "Eletrônicos")
            ) /* expects no results*/);

            TestLocations(env, new FilterGroup(GroupingClause.And,
                new RangeFilter("Price", 3000, 4000),
                new FilterGroup(GroupingClause.Or,
                    new ValueFilter("Category", "Eletrodomésticos"),
                    new ValueFilter("Category", "Eletrônicos"))
            ), "Impact Hub Curitiba");
        }
    }
}
