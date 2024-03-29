﻿using SmartSearch.Abstractions;
using LuceneDocument = Lucene.Net.Documents.Document;

namespace SmartSearch.LuceneNet.Internals.Converters
{
    internal interface IDocumentConverter
    {
        LuceneDocument Convert(InternalSearchDomain domain, InternalDocument sourceDocument);

        IDocument Convert(InternalSearchDomain domain, LuceneDocument luceneDocument);
    }
}