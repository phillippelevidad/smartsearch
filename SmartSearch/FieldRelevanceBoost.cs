using SmartSearch.Abstractions;

namespace SmartSearch
{
    public static class FieldRelevanceBoost
    {
        public static float GetBoostValue(this FieldRelevance relevance)
        {
            switch (relevance)
            {
                case FieldRelevance.Higher:
                    return 9f;
                case FieldRelevance.High:
                    return 3f;
                default:
                    return 1f;
            }
        }
    }
}
