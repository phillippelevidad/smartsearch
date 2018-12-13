using SmartSearch.Abstractions;
using System.Diagnostics;

namespace SmartSearch
{
    [DebuggerDisplay("{FieldName} = {SingleValue} | {RangeFrom} to {RangeTo}")]
    public class Filter : IFilter
    {
        private FilterType filterType;
        private object rangeFrom;
        private object rangeTo;
        private object singleValue;
        public string FieldName { get; set; }

        public FilterType FilterType
        {
            get => filterType;
            set
            {
                filterType = value;

                switch (filterType)
                {
                    case FilterType.SingleValue:
                        rangeFrom = rangeTo = null;
                        break;

                    case FilterType.Range:
                        singleValue = null;
                        break;

                    default:
                        throw new UnknownQueryFilterTypeException(filterType);
                }
            }
        }

        public object RangeFrom
        {
            get => rangeFrom;
            set
            {
                rangeFrom = value;
                singleValue = null;
                filterType = FilterType.Range;
            }
        }

        public object RangeTo
        {
            get => rangeTo;
            set
            {
                rangeTo = value;
                singleValue = null;
                filterType = FilterType.Range;
            }
        }

        public object SingleValue
        {
            get => singleValue;
            set
            {
                singleValue = value;
                rangeFrom = rangeTo = null;
                filterType = FilterType.SingleValue;
            }
        }

        public Filter()
        {
        }

        public Filter(string fieldName, object value)
        {
            FieldName = fieldName;
            SingleValue = value;
        }

        public Filter(string fieldName, object from, object to)
        {
            FieldName = fieldName;
            RangeFrom = from;
            RangeTo = to;
        }
    }
}