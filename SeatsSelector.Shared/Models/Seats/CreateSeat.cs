using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatsSelector.Shared.Models.Seats
{
    public class CreateSeat
    {
        public int RoomId { get; set; }
        public int Col { get; set; }
        public int Row { get; set; }
    }
}
