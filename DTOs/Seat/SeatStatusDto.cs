namespace StudySpace.API.DTOs.Seat
{
    public class SeatStatusDto
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public string Floor { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal MonthlyPrice { get; set; }
        public bool IsBooked { get; set; }
        public bool IsActive { get; set; }
    }
}