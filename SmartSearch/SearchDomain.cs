using SmartSearch.Abstractions;

namespace SmartSearch
{
    public class SearchDomain : ISearchDomain
    {
        public string Name { get; set; }

        public IField[] Fields { get; set; }

        public IAnalysisSettings AnalysisSettings { get; set; }

        public SearchDomain()
        {
            Fields = new IField[0];
        }

        public SearchDomain(string name, IAnalysisSettings analysisSettings = null)
        {
            Name = name;
            AnalysisSettings = analysisSettings;
            Fields = new IField[0];
        }

        public SearchDomain(string name, params IField[] fields)
        {
            Name = name;
            Fields = fields ?? new IField[0];
        }

        public SearchDomain(string name, IAnalysisSettings analysisSettings, params IField[] fields)
        {
            Name = name;
            AnalysisSettings = analysisSettings;
            Fields = fields ?? new IField[0];
        }
    }
}
