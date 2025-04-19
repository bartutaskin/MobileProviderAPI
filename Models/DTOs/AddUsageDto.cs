namespace MobileProvider.Models.DTOs
{
    public class AddUsageDto
    {
        public string SubscriberNo { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }

    }
}
