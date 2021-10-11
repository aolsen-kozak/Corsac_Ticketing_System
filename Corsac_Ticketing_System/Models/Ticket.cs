using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Corsac_Ticketing_System.Models
{
    public enum Statuses
    {
        Waiting_For_Staff_Response, 
        Waiting_For_Customer, 
        On_Hold, 
        Cancelled, 
        Completed 
    }

    public enum Departments
    {
        Support,
        Billing,
        Purchasing,
        Vendors,
        Marketing
    }

    public class Ticket
    {
        [Key]
        public Int64 Id { get; set; }

        public string ReferenceId { get; set; }
        public Statuses Status { get; set; }
        public int? StaffId { get; set; }
        public int CustomerId { get; set; }
        public string SubjectLine { get; set; }
        public string IssueDescription { get; set; }
        public Departments Department { get; set; }

        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Staff Staff { get; set; }
    }
}