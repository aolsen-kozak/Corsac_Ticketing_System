using Corsac_Ticketing_System.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Corsac_Ticketing_System.Data_Access
{
    public class CorsacTicketContext : DbContext
    {
        public CorsacTicketContext() : base("CorsacContext") { }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<TicketHistory> TicketHistories { get; set; }
        public DbSet<Customer> Customers { get; set; }
            
    }
}