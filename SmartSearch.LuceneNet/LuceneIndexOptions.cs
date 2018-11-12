using SmartSearch.LuceneNet.Analysis;
using System;

namespace SmartSearch.LuceneNet
{
    public class LuceneIndexOptions
    {
        static readonly string DefaultIndexDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public bool ForceRecreate { get; private set; } = false;

        public string IndexDirectory { get; private set; } = DefaultIndexDirectory;

        public bool IndexInMemory { get; private set; } = false;

        public IAnalyzerFactory AnalyzerFactory { get; private set; } = new StandardAnalyzerFactory();

        public LuceneIndexOptions UseAnalyzerFactory(IAnalyzerFactory analyzerFactory)
        {
            AnalyzerFactory = analyzerFactory ?? throw new ArgumentNullException(nameof(analyzerFactory));
            return this;
        }

        public LuceneIndexOptions UsePhysicalIndexDirectory(string indexDirectory, bool forceRecreate = false)
        {
            IndexDirectory = indexDirectory ?? throw new ArgumentNullException(nameof(indexDirectory));
            ForceRecreate = forceRecreate;
            IndexInMemory = false;
            return this;
        }

        public LuceneIndexOptions UseInMemoryDirectory()
        {
            IndexInMemory = true;
            return this;
        }
    }
}
