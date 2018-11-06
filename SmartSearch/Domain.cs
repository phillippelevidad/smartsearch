using SmartSearch.Abstractions;

namespace SmartSearch
{
    public class Domain : IDomain
    {
        public string Name { get; set; }

        public IField[] Fields { get; set; }

        public IAnalysisSettings AnalysisSettings { get; set; }

        public Domain()
        {
            Fields = new Field[0];
        }

        public Domain(string name, IAnalysisSettings analysisSettings = null)
        {
            Name = name;
            AnalysisSettings = analysisSettings;
            Fields = new Field[0];
        }

        public Domain(string name, params IField[] fields)
        {
            Name = name;
            Fields = fields;
        }

        public Domain(string name, IAnalysisSettings analysisSettings, params IField[] fields)
        {
            Name = name;
            AnalysisSettings = analysisSettings;
            Fields = fields;
        }
    }
}
