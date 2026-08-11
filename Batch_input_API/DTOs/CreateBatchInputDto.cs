namespace Batch_input_API.DTOs
{
    public class CreateBatchInputDto
    {
        public string TerminalID { get; set; } = string.Empty;
        public string Machine { get; set; } = string.Empty;
        public bool Start { get; set; }
        public string msnv { get; set; } = string.Empty;
        public string? Shift { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public List<String> ListPO { get; set; } = new();


    }
}
