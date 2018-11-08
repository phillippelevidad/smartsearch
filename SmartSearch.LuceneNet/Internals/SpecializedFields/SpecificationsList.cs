namespace SmartSearch.LuceneNet.Internals.SpecializedFields
{
    static class SpecificationsList
    {
        static ISpecializedFieldSpecification[] list;

        public static ISpecializedFieldSpecification[] All
        {
            get
            {
                if (list == null)
                    Build();

                return list;
            }
        }

        static void Build() => list = new ISpecializedFieldSpecification[]
        {
            new SynonymFieldSpecification()
        };
    }
}
