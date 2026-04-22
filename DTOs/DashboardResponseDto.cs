namespace StudySpace.API.DTOs
{
    public class DashboardResponseDto
    {
        public int TotalSeats { get; set; }
        public int OccupiedSeats { get; set; }
        public int AvailableSeats { get; set; }
        public int PendingBookings { get; set; }
        public int OverduePayments { get; set; }
        public decimal RevenueThisMonth { get; set; }
        public int RenewalsThisWeek { get; set; }
    }
}