namespace TestTask1.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }

        public virtual List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public int ProviderId { get; set; }

        public virtual Provider Provider { get; set; }
    }
}
