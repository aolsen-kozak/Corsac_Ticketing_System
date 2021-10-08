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
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public string OldStaffId { get; set; }
        public string NewStaffId { get; set; }
        public string Comment { get; set; }
    }
}