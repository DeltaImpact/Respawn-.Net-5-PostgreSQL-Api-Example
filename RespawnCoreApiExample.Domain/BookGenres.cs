using System.Collections.Generic;

namespace RespawnCoreApiExample.Domain
{
    public static class BookGenres
    {
        public static string Fantasy = "Fantasy";
        public static string HistoricalFiction = "Historical fiction";
        public static string ContemporaryFiction = "Contemporary fiction";
        public static string Mystery = "Mystery";
        public static string ScienceFiction = "Science fiction";

        public static List<string> GetAllGenres()
        {
            return new() {Fantasy, HistoricalFiction, ContemporaryFiction, Mystery, ScienceFiction};
        }
    }
}