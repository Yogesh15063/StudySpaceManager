using StudySpace.API.Models;

namespace StudySpace.API.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (context.Seats.Any()) return;

            var seats = new List<Seat>();

            // Library 1 — 108 seats
            for (int i = 1; i <= 108; i++)
            {
                seats.Add(new Seat
                {
                    SeatNumber = $"L1-{i:D3}",
                    Floor = "Library1",
                    Type = "Standard",
                    MonthlyPrice = 899,
                    IsActive = true
                });
            }

            // Library 2 — 29 seats
            for (int i = 1; i <= 29; i++)
            {
                seats.Add(new Seat
                {
                    SeatNumber = $"L2-{i:D2}",
                    Floor = "Library2",
                    Type = "Premium",
                    MonthlyPrice = 1199,
                    IsActive = true
                });
            }

            context.Seats.AddRange(seats);
            await context.SaveChangesAsync();
        }
    }
}