namespace TeaStoreApi.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int Qty { get; set; }
        public double TotalAmount { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
    }
}
