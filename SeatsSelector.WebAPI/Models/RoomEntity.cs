namespace SeatsSelector.WebAPI.Models
{
    public class RoomEntity
    {
        public int Id { get; set; }

        public int ColCount { get; set; }
        public int RowCount { get; set; }


        // --

        public List<SeatEntity> Seats { get; set; }
    }
}
