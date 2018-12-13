using Lucene.Net.Store;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Internals;
using SmartSearch.LuceneNet.Internals.Helpers;
using System;

namespace SmartSearch.LuceneNet
{
    public class MemoryIndexContext : IIndexContext
    {
        private readonly BaseIndexContext baseContext;

        public MemoryIndexContext()
        {
            baseContext = new BaseIndexContext(InitializeCompositeIndex);
        }

        public void Dispose() => baseContext.Dispose();

        public object GetContext() => baseContext.GetContext();

        private CompositeIndex InitializeCompositeIndex()
        {
            var facetsDir = new RAMDirectory();
            var indexDir = new RAMDirectory();
            return new CompositeIndex(facetsDir, indexDir);
        }
    }

    public class PhysicalIndexContext : IIndexContext
    {
        private readonly BaseIndexContext baseContext;

        public bool ForceRecreate { get; }
        public string IndexDirectory { get; }

        public PhysicalIndexContext(string indexDirectory, bool forceRecreate)
        {
            baseContext = new BaseIndexContext(InitializeCompositeIndex);
            IndexDirectory = indexDirectory;
            ForceRecreate = forceRecreate;
        }

        public void Dispose() => baseContext.Dispose();

        public object GetContext() => baseContext.GetContext();

        private CompositeIndex InitializeCompositeIndex()
        {
            var facetsPath = IndexDirectoryHelper.GetFacetsDirectoryPath(IndexDirectory);
            var facetsDir = FSDirectory.Open(facetsPath);

            var indexPath = IndexDirectoryHelper.GetDirectoryPath(IndexDirectory);
            var indexDir = FSDirectory.Open(indexPath);

            return new CompositeIndex(facetsDir, indexDir);
        }
    }

    internal class BaseIndexContext : IIndexContext
    {
        private readonly Func<CompositeIndex> initializeCompositeIndex;

        private CompositeIndex compositeIndex;

        public BaseIndexContext(Func<CompositeIndex> initializeCompositeIndex)
        {
            this.initializeCompositeIndex = initializeCompositeIndex;
        }

        public void Dispose() => compositeIndex?.Dispose();

        public object GetContext()
        {
            if (compositeIndex == null)
                compositeIndex = initializeCompositeIndex.Invoke();

            return compositeIndex;
        }
    }
}