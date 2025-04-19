using System.ComponentModel.DataAnnotations;

namespace SE4453_MobileProvider.Models.DTOs
{
    public class BillDetailsDto
    {
        public int Month { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PhoneCharge { get; set; }
        public decimal InternetCharge { get; set; }
        public bool PaidStatus { get; set; }
    }
}
