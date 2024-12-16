namespace SeatsSelector.Shared.Models.Seats
{
    public class Seat
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int? UserId { get; set; }

        public int Col { get; set; }
        public int Row { get; set; }
        public bool IsOccupied { get; set; }
    }
}
