using SmartSearch.Abstractions;
using LuceneDocument = Lucene.Net.Documents.Document;

namespace SmartSearch.LuceneNet
{
    public interface IDocumentConverter
    {
        LuceneDocument Convert(ISearchDomain domain, IDocument sourceDocument);

        IDocument Convert(ISearchDomain domain, LuceneDocument luceneDocument);
    }
}