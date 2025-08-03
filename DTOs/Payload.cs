using System.Text.Json.Serialization;

namespace External_API_Integration.DTOs
{
    public class Payload
    {
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }
    }
}
