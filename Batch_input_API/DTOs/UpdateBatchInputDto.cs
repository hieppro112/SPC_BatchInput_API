namespace Batch_input_API.DTOs
{
    public class UpdateBatchInputDto
    {
        public string TerminalID { get; set; } = string.Empty;

        public string Machine { get; set; } = string.Empty;

        public bool Start { get; set; }

        public string Msnv { get; set; } = string.Empty;
        public bool isComplate { get; set; } = false;

        public string? Shift { get; set; }
    }
}
