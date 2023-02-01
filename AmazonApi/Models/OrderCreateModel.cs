namespace AmazonApi.Models
{
    public class OrderCreateModel
    {
        public string UserId { get; set; }
        public double TotalPrice { get; set; }
        public string PaymentMethod { get; set; }

        
        public string Address { get; set; }

        public string Street { get; set; }


        public OrderItemModel orderItems { set; get; }
    }
}
