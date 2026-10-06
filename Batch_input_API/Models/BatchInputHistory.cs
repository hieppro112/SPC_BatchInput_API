namespace Batch_input_API.Models
{
    public class BatchInputHistory
    {
        public int ID { get; set; }
        public string TerminalID { get; set; } = string.Empty;
        public string Machine { get; set; }=string.Empty;
        public bool start { get; set; } = false;
        public string Msnv { get; set; } = string.Empty;
        public string Shift { get; set; } = string.Empty;
        public bool isComplate { get; set; } = false;
        public DateTime? DateCreated { get; set; } = DateTime.Now;
        public int numRetry { get; set; }=0;
        public string description { get; set; } = string.Empty;
        public bool isDelete { get; set; } = false;
        public DateTime RUNTIME { get; set; } = DateTime.Now;
        public List<ListPO> ListPOs { get; set; } = new List<ListPO>();

    }
}
