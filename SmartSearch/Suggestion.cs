using SmartSearch.Abstractions;
using System.Collections.Generic;

namespace SmartSearch
{
    public class Suggestion : ISuggestion
    {
        public string Text { get; set; }

        public int Weight { get; set; }

        public IDictionary<string, object> ExtraData { get; set; }

        public Suggestion() : this(null, 0, null)
        {
        }

        public Suggestion(string text, int weight, IDictionary<string, object> extraData = null)
        {
            Text = text;
            Weight = weight;
            ExtraData = extraData ?? new Dictionary<string, object>();
        }
    }
}
