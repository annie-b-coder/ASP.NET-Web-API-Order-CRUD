namespace TestTask1.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }       
        public string Name { get; set; }
        public decimal Quantity { get; set; }        
        public string Unit {  get; set; }
    }
}
