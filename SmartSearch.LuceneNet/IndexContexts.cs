using Lucene.Net.Store;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Internals;
using SmartSearch.LuceneNet.Internals.Helpers;
using System;

namespace SmartSearch.LuceneNet
{
    public class MemoryIndexContext : IIndexContext
    {
        readonly BaseIndexContext baseContext;

        public MemoryIndexContext()
        {
            baseContext = new BaseIndexContext(InitializeCompositeIndex);
        }

        public object GetContext() => baseContext.GetContext();

        public void Dispose() => baseContext.Dispose();

        CompositeIndex InitializeCompositeIndex()
        {
            var facetsDir = new RAMDirectory();
            var indexDir = new RAMDirectory();
            return new CompositeIndex(facetsDir, indexDir);
        }
    }

    public class PhysicalIndexContext : IIndexContext
    {
        readonly BaseIndexContext baseContext;

        public string IndexDirectory { get; }

        public bool ForceRecreate { get; }

        public PhysicalIndexContext(string indexDirectory, bool forceRecreate)
        {
            baseContext = new BaseIndexContext(InitializeCompositeIndex);
            IndexDirectory = indexDirectory;
            ForceRecreate = forceRecreate;
        }

        public object GetContext() => baseContext.GetContext();

        public void Dispose() => baseContext.Dispose();

        CompositeIndex InitializeCompositeIndex()
        {
            var facetsPath = IndexDirectoryHelper.GetFacetsDirectoryPath(IndexDirectory);
            var facetsDir = FSDirectory.Open(facetsPath);

            var indexPath = IndexDirectoryHelper.GetDirectoryPath(IndexDirectory);
            var indexDir = FSDirectory.Open(indexPath);

            return new CompositeIndex(facetsDir, indexDir);
        }
    }

    class BaseIndexContext : IIndexContext
    {
        readonly Func<CompositeIndex> initializeCompositeIndex;

        CompositeIndex compositeIndex;

        public BaseIndexContext(Func<CompositeIndex> initializeCompositeIndex)
        {
            this.initializeCompositeIndex = initializeCompositeIndex;
        }

        public object GetContext()
        {
            if (compositeIndex == null)
                compositeIndex = initializeCompositeIndex.Invoke();

            return compositeIndex;
        }

        public void Dispose() => compositeIndex?.Dispose();
    }
}
