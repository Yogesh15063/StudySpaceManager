namespace StudySpace.API.DTOs.Booking
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int SeatId { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public int Month { get; set; }
        public int Year { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}