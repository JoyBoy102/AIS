using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AIS.Structs
{
    public class ErrorResponse
    {
        [JsonPropertyName("detail")]
        public ErrorDetail[] Detail { get; set; }
    }

    public class ErrorDetail
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("loc")]
        public string[] Loc { get; set; }
        [JsonPropertyName("msg")]
        public string Msg { get; set; }
        [JsonPropertyName("input")]
        public string Input { get; set; }
        [JsonPropertyName("ctx")]
        public ErrorContext Ctx { get; set; }
    }

    public class ErrorContext
    {
        [JsonPropertyName("min_length")]
        public int MinLength { get; set; }
    }
}
