namespace TestTask1.Models.DTO
{
    public class DTOOrder
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }

        public List<DTOOrderItem> OrderItems { get; set; } 
        public int ProviderId { get; set; }

        public string ProviderName { get; set; }
    }
}
