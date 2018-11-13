using Lucene.Net.Store;
using SmartSearch.Abstractions;

namespace SmartSearch.LuceneNet.Internals
{
    class IndexContextWrapper
    {
        public IIndexContext WrappedContext { get; }

        public IndexContextWrapper(IIndexContext indexContext)
        {
            WrappedContext = indexContext;
        }

        public CompositeIndex CompositeIndex => WrappedContext.GetContext() as CompositeIndex;

        public Directory FacetsDirectory => CompositeIndex?.FacetsDirectory;

        public Directory IndexDirectory => CompositeIndex?.IndexDirectory;
    }
}
