using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Windows;

namespace AIS.Structs
{
    public class AgronomicRuleModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("type_crop")]
        public string CultureType { get; set; }

        [JsonPropertyName("rule_params")]
        public string Rule { get; set; }

        public override string ToString()
        {
            return CultureType.ToString();
        }
    }
}