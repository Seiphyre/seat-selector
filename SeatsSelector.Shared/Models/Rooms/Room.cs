using SeatsSelector.Shared.Models.Seats;

namespace SeatsSelector.Shared.Models.Rooms
{
    public class Room
    {
        public int Id { get; set; }

        public List<Seat> Seats { get; set; }
        public int ColCount { get; set; }
        public int RowCount { get; set; }


        public Seat GetSeatAt(int row, int col)
        {
            return Seats?.FirstOrDefault(seat => seat.Row == row && seat.Col == col);
        }
    }
}
