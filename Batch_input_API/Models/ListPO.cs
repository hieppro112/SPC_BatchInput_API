namespace Batch_input_API.Models
{
    public class ListPO
    {
        public int ID { get; set; }
        public string Po { get; set; }
        public int IDGroup { get; set; }
        public BatchInputHistory BatchInputHistory { get; set; } = new();
    }
}
