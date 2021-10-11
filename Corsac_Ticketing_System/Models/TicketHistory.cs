using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Corsac_Ticketing_System.Models
{
    public class TicketHistory
    {
        public Int64 Id { get; set; }
        public Int64 TicketId { get; set; }
        public Statuses OldStatus { get; set; }
        public Statuses NewStatus { get; set; }
        public int? OldStaffId { get; set; }
        public int? NewStaffId { get; set; }
        public string Comment { get; set; }
        public bool StaffEdit { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}