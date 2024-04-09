using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class BookApiResponse
    {
        public int Start { get; set; }
        public int NumFound { get; set; }
        public List<ApiBook> Docs { get; set; }
    }

    public class ApiBook
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("cover_i")]
        public int CoverId { get; set; } 

        [JsonPropertyName("has_fulltext")]
        public bool HasFulltext { get; set; }

        [JsonPropertyName("edition_count")]
        public int EditionCount { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("author_name")]
        public List<string> AuthorNames { get; set; }

        [JsonPropertyName("first_publish_year")]
        public int FirstPublishYear { get; set; }
    }
}
