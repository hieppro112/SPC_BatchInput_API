namespace Batch_input_API.DTOs
{
    public class PODto
    {
        public int ID { get; set; }
        public string Po { get; set; }
        public int IDGroup { get; set; }
        public bool isComplate { get; set; } = false;
        public string DesPO { get; set; } = string.Empty;
    }
}
