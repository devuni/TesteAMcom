using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Questao2.Models
{
    public class ApiResponse
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("per_page")]
        public int PerPage { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("data")]
        public List<MatchData> Data { get; set; }
    }
}
