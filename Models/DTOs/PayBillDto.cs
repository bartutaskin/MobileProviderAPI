namespace SE4453_MobileProvider.Models.DTOs
{
    public class PayBillDto
    {
        public string SubscriberNo { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}
