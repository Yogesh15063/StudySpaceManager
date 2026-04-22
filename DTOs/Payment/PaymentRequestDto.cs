namespace StudySpace.API.DTOs.Payment
{
    public class PaymentRequestDto
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
    }
}