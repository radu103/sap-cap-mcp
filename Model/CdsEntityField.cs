using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CdsServiceClient.Model
{
    public class CdsEntityField
    {
        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("Type")]
        public string Type { get; set; } = "String";

        [JsonPropertyName("isNullable")]
        [DefaultValue(false)]
        public bool IsNullable { get; set; } = false;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("length")]
        public string? Length { get; set; }

        [JsonPropertyName("precision")]
        public string? Precision { get; set; }
    }
}