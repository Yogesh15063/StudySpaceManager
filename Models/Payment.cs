namespace StudySpace.API.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? PaidOn { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public Booking Booking { get; set; } = null!;
    }
}