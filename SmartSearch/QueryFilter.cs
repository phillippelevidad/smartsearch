using SmartSearch.Abstractions;
using System.Diagnostics;

namespace SmartSearch
{
    [DebuggerDisplay("{FieldName} = {SingleValue} | {RangeFrom} to {RangeTo}")]
    public class QueryFilter : IQueryFilter
    {
        object singleValue;
        object rangeFrom;
        object rangeTo;
        QueryFilterType filterType;

        public string FieldName { get; set; }

        public object SingleValue
        {
            get => singleValue;
            set
            {
                singleValue = value;
                rangeFrom = rangeTo = null;
                filterType = QueryFilterType.SingleValue;
            }
        }

        public object RangeFrom
        {
            get => rangeFrom;
            set
            {
                rangeFrom = value;
                singleValue = null;
                filterType = QueryFilterType.Range;
            }
        }

        public object RangeTo
        {
            get => rangeTo;
            set
            {
                rangeTo = value;
                singleValue = null;
                filterType = QueryFilterType.Range;
            }
        }

        public QueryFilterType FilterType
        {
            get => filterType;
            set
            {
                filterType = value;

                switch (filterType)
                {
                    case QueryFilterType.SingleValue:
                        rangeFrom = rangeTo = null;
                        break;
                    case QueryFilterType.Range:
                        singleValue = null;
                        break;
                    default:
                        throw new UnknownQueryFilterTypeException(filterType);
                }
            }
        }

        public QueryFilter()
        {
        }

        public QueryFilter(string fieldName, object value)
        {
            FieldName = fieldName;
            SingleValue = value;
        }

        public QueryFilter(string fieldName, object from, object to)
        {
            FieldName = fieldName;
            RangeFrom = from;
            RangeTo = to;
        }
    }
}
