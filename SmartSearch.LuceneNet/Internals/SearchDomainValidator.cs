using SmartSearch.Abstractions;
using System.Linq;
using System.Text.RegularExpressions;

namespace SmartSearch.LuceneNet.Internals
{
    internal class SearchDomainValidator
    {
        private static readonly Regex regexName = new Regex(@"^[\w_]+$", RegexOptions.Compiled);

        public void Validate(ISearchDomain domain)
        {
            ValidateDomainName(domain);
            ValidateFields(domain);
        }

        private void ValidateDomainName(ISearchDomain domain)
        {
            if (domain.Name == null || !regexName.IsMatch(domain.Name))
                throw new InvalidSearchDomainNameException(domain.Name);
        }

        private void ValidateField(ISearchDomain domain, IField field)
        {
            if (field.Name == null || !regexName.IsMatch(field.Name))
                throw new InvalidFieldNameException(field.Name);

            foreach (var name in domain.Fields.Select(f => f.Name))
                if (domain.Fields.Count(f => f.Name.Equals(name)) > 1)
                    throw new DuplicatedFieldException(name);
        }

        private void ValidateFields(ISearchDomain domain)
        {
            foreach (var field in domain.Fields)
                ValidateField(domain, field);
        }
    }
}