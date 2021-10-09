using Corsac_Ticketing_System.Models;
using Corsac_Ticketing_System.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Corsac_Ticketing_System.Data_Access
{
    public class CorsacTicketInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<CorsacTicketContext>
    {
        protected override void Seed(CorsacTicketContext context) {

            Utilities.Utilities _utilities = new Utilities.Utilities();
            string passHash = _utilities.GetMD5Hash("temp123");

            var staffMembers = new List<Staff>
            {
                new Staff{UserName = "testUser1", Password = passHash },
                new Staff{UserName = "testUser2", Password = passHash},
                new Staff{UserName = "testUser3", Password = passHash}
            };

            staffMembers.ForEach(s => context.Staffs.Add(s));

            context.SaveChanges();

            var customers = new List<Customer>
            {
                new Customer{Name = "Joe Average", EmailAddress = "" },
                new Customer{Name = "Suzy Simple", EmailAddress = "" },
                new Customer{Name = "Carl Carbon", EmailAddress = "" },
            };

            customers.ForEach(c => context.Customers.Add(c));

            context.SaveChanges();

            var tickets = new List<Ticket> 
            {
                new Ticket{Status = Statuses.Waiting_For_Customer, StaffId = 1, CustomerId = 1, Department = Departments.Billing,  SubjectLine = "Double Billed?", IssueDescription = "I think I've been double billed" },
                new Ticket{Status = Statuses.On_Hold, StaffId = 2, CustomerId = 1, Department = Departments.Marketing,  SubjectLine = "Don't Want Flyers!", IssueDescription = "Please stop sending flyers to my house.  I am not interested in your products!" },
                new Ticket{Status = Statuses.Completed, StaffId = 1, CustomerId = 2, Department = Departments.Support,  SubjectLine = "New Computer Needed", IssueDescription = "My computer blue screens about once a day and I think I need a new one." },
                new Ticket{Status = Statuses.Waiting_For_Staff_Response, StaffId = 3, CustomerId = 3, Department = Departments.Billing,  SubjectLine = "Update Credit Card", IssueDescription = "I need to update my credit card on file.  I tried through the website but it's not working." },
            };

            tickets.ForEach(t => {
                string referenceId = _utilities.GenerateReferenceId();
                while(context.Tickets.Any<Ticket>(a => a.ReferenceId == referenceId))
                {
                    referenceId = _utilities.GenerateReferenceId();
                }

                t.ReferenceId = referenceId;

                context.Tickets.Add(t);
            });

            context.SaveChanges();

            var ticketHistories = new List<TicketHistory>
            {
                new TicketHistory{TicketId = 1, OldStaffId = null, NewStaffId = 1, OldStatus = Statuses.Waiting_For_Staff_Response.ToString(), NewStatus = Statuses.Waiting_For_Customer.ToString(), Comment = "Sure, would you mind passing me your account number?"},
                new TicketHistory{TicketId = 2, OldStaffId = null, NewStaffId = 2, OldStatus = Statuses.Waiting_For_Staff_Response.ToString(), NewStatus = Statuses.On_Hold.ToString()},
                new TicketHistory{TicketId = 3, OldStaffId = null, NewStaffId = 1, OldStatus = Statuses.Waiting_For_Staff_Response.ToString(), NewStatus = Statuses.Completed.ToString(), Comment = "Hi Suzy, I ran a repair on your Windows system last night and things look good. I'll close this for now and feel free to re-open if needed."},
            };

            ticketHistories.ForEach(th => context.TicketHistories.Add(th));

            context.SaveChanges();
        }    
    }
}