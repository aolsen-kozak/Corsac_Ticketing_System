using Corsac_Ticketing_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Corsac_Ticketing_System.ViewModels
{
    public class EditTicket
    {
        public Ticket Ticket { get; set; }
        public string Comment { get; set; }
        public bool StaffEdit { get; set; }

        public ICollection<TicketHistory> TicketHistories { get; set; }
    }
}