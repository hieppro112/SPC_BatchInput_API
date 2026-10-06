using System.Text.Json.Serialization;

namespace Batch_input_API.Models
{
    public class ListPO
    {
        public int ID { get; set; }
        public string? Po { get; set; }
        public int IDGroup { get; set; }
        public bool? isComplate { get; set; } = false;
        public string? DescriptionPO { get; set; } = string.Empty;
        [JsonIgnore]
        public BatchInputHistory? BatchInputHistory { get; set; }
    }
}
