namespace TestTask1.Models.DTO
{
    public class DTOOrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }

        public DTOOrderItem(int id, int orderId, string orderNumber, string name, decimal quantity, string unit)
        {
            this.Id = id;
            this.OrderId = orderId;
            this.OrderNumber = orderNumber;
            this.Name = name;
            this.Quantity = quantity;
            this.Unit = unit;
        }
    }
}
