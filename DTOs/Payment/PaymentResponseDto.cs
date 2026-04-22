namespace StudySpace.API.DTOs.Payment
{
    public class PaymentResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int BookingId { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? PaidOn { get; set; }
    }
}