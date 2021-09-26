using System.ComponentModel.DataAnnotations;

namespace CreditCardPaymentAPI.DTOs
{
    public class CreditCardPostDto
    {
        [Required]
        public string CardHolderName { get; set; }

        [Required]
        [RegularExpression("0[1-9]|1[0-2]"), MaxLength(2), MinLength(2)]
        public string ExpiresMonth { get; set; }

        [Required]
        [RegularExpression("[0-9]*"), MaxLength(2), MinLength(2)]
        public string ExpiresYear { get; set; }

        [Required]
        [RegularExpression("[1-9]*"), MaxLength(16), MinLength(16)]
        public string CardNumber { get; set; }

        [Required]
        [Range(100, 999)]
        public int Cvc { get; set; }
    }
}
