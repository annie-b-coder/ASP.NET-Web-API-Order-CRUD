namespace TestTask1.Models.DTO
{
    public class DTONewOrder
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int ProviderId { get; set; }

        public List<DTONewOrderItem> OrderItems { get; set; }
    }
}
