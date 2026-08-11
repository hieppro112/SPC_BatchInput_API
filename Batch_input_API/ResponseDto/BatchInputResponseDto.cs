using Batch_input_API.DTOs;

namespace Batch_input_API.ResponseDto
{
    public class BatchInputResponseDto
    {
        public int Id { get; set; }

        public string TerminalID { get; set; } = "";

        public string Machine { get; set; } = "";

        public bool Start { get; set; }

        public string Msnv { get; set; } = "";

        public string? Shift { get; set; }
        public bool? isComplate { get; set; } 
        public DateTime? DateCreated { get; set; } = DateTime.Now;


        public List<PODto> ListPO { get; set; } = new();
    }
}
