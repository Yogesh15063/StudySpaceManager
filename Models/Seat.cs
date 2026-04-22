namespace StudySpace.API.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public string Floor { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal MonthlyPrice { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
