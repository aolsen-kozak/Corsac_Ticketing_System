using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Corsac_Ticketing_System.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}