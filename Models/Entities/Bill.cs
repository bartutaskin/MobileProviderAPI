using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MobileProvider.Models.Entities
{
    public class Bill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Subscriber")]
        [Required]  
        public int SubscriberId { get; set; }
        public Subscriber Subscriber { get; set; }

        [Required]  
        public int Month { get; set; }

        [Required]  
        public int Year { get; set; }

        [Required] 
        public decimal TotalAmount { get; set; }

        public decimal PhoneCharge { get; set; }  
        public decimal InternetCharge { get; set; } 

        [Required]  
        public bool PaidStatus { get; set; }
    }

}
