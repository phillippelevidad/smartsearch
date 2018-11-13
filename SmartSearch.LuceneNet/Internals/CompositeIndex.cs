using Lucene.Net.Store;
using System;

namespace SmartSearch.LuceneNet.Internals
{
    class CompositeIndex : IDisposable
    {
        bool disposed;

        internal Directory FacetsDirectory { get; }

        internal Directory IndexDirectory { get; }

        public CompositeIndex(Directory facetsDirectory, Directory indexDirectory)
        {
            FacetsDirectory = facetsDirectory ?? throw new ArgumentNullException(nameof(facetsDirectory));
            IndexDirectory = indexDirectory ?? throw new ArgumentNullException(nameof(indexDirectory));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !disposed)
            {
                FacetsDirectory.Dispose();
                IndexDirectory.Dispose();
                disposed = true;
            }
        }
    }
}
