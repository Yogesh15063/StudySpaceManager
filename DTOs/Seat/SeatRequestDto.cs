namespace StudySpace.API.DTOs.Seat
{
    public class SeatRequestDto
    {
        public string SeatNumber { get; set; } = string.Empty;
        public string Floor { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal MonthlyPrice { get; set; }
    }
}