namespace SeatsSelector.WebAPI.Models
{
    public class SeatEntity
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int? UserId { get; set; }

        public int Col { get; set; }
        public int Row { get; set; }



        // --

        public UserEntity User { get; set; }
        public RoomEntity Room { get; set; }
    }
}
