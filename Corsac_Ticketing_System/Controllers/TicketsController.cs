using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Corsac_Ticketing_System.Data_Access;
using Corsac_Ticketing_System.Models;
using Corsac_Ticketing_System.ViewModels;


namespace Corsac_Ticketing_System.Controllers
{
    public class TicketsController : Controller
    {
        private readonly CorsacTicketContext _corsacContext;
        private readonly Utilities.Utilities _utilities;

        public TicketsController()
        {
            _corsacContext = new CorsacTicketContext();
            _utilities = new Utilities.Utilities();
        }
        // GET: Tickets
        public async Task<ActionResult> Index( string list)
        {

            switch (list)
            {
                case "newUnassigned":
                    {
                        return View(await _corsacContext.Tickets.Where(t => t.StaffId == null 
                            && t.Status == Statuses.Waiting_For_Staff_Response).ToListAsync());
                    }
                case "open":
                    {
                        return View(await _corsacContext.Tickets.Where(t => t.Status != Statuses.Waiting_For_Staff_Response || 
                            (t.Status == Statuses.Waiting_For_Staff_Response && t.TicketHistories.Count > 0)).ToListAsync());
                    }
                    
                case "onHold":
                    {
                        return View(await _corsacContext.Tickets.Where(t => t.Status == Statuses.On_Hold).ToListAsync());
                    }
                    
                case "closed":
                    {
                        return View(await _corsacContext.Tickets.Where(t => t.Status == Statuses.Completed 
                            || t.Status == Statuses.Cancelled).ToListAsync());
                    }
                default:
                    return View(await _corsacContext.Tickets.ToListAsync());
            }
            
        }


        // GET: Tickets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CustomerName, CustomerEmailAddress, Department, SubjectLine,IssueDescription")] CreateTicket createTicket)
        {
            if (ModelState.IsValid)
            {
                Customer customer = await _corsacContext.Customers.Where(c => c.EmailAddress == createTicket.CustomerEmailAddress).FirstOrDefaultAsync();

                if (customer == null)
                {
                    //Customer doesn't exist yet, let's create them
                    customer = new Customer
                    {
                        Name = createTicket.CustomerName,
                        EmailAddress = createTicket.CustomerEmailAddress
                    };

                    _corsacContext.Customers.Add(customer);
                    _corsacContext.SaveChanges();

                }


                Ticket ticket = new Ticket
                {
                    ReferenceId = GenerateUniqueReferenceId(),
                    Status = Statuses.Waiting_For_Staff_Response,
                    CustomerId = customer.Id,
                    SubjectLine = createTicket.SubjectLine,
                    IssueDescription = createTicket.IssueDescription,
                    Department = createTicket.Department
                };

                _corsacContext.Tickets.Add(ticket);
                var result = await _corsacContext.SaveChangesAsync();

                if (result > 0)
                {
                    SendTicketCreatedEmail(ticket.ReferenceId, customer);
                    return RedirectToAction("Edit", new { refTicketId = ticket.ReferenceId });
                }
                else
                {
                    ViewBag.ErrorMessage = "Something went wrong, please check your entered values and try again";
                    return View(createTicket);
                }
            }

            return View(createTicket);
        }

        // GET: Tickets/Edit/ABC123EFG4
        public async Task<ActionResult> Edit(string refTicketId)
        {
            if (refTicketId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ticket ticket = null;

            if(refTicketId.Length == 10 && !int.TryParse(refTicketId, out _))
            {
                ticket = await _corsacContext.Tickets.Where(t => t.ReferenceId == refTicketId).FirstOrDefaultAsync();
            }
            else
            {
                ticket = await _corsacContext.Tickets.FindAsync(int.Parse(refTicketId));
            }
            

            if (ticket == null)
            {
                return HttpNotFound();
            }

            EditTicket editTicket = new EditTicket
            {
                Ticket = ticket,
                TicketHistories = _corsacContext.TicketHistories.Where(th => th.TicketId == ticket.Id).ToList()
            };

            return View(editTicket);
        }


        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( EditTicket editedTicket)
        {
            if (ModelState.IsValid)
            {
                Ticket ticket = _corsacContext.Tickets.Find(editedTicket.Ticket.Id);

                TicketHistory ticketHistory = new TicketHistory
                {
                    TicketId = editedTicket.Ticket.Id,
                    OldStatus = ticket.Status,
                    NewStatus = (!editedTicket.StaffEdit) ? Statuses.Waiting_For_Staff_Response : editedTicket.Ticket.Status,
                    OldStaffId = ticket.StaffId,
                    NewStaffId = editedTicket.Ticket.StaffId,
                    Comment = editedTicket.Comment,
                    StaffEdit = editedTicket.StaffEdit,
                    TimeStamp = DateTime.UtcNow
                };

                _corsacContext.TicketHistories.Add(ticketHistory);

                ticket.Status = (!editedTicket.StaffEdit) ? Statuses.Waiting_For_Staff_Response : editedTicket.Ticket.Status;
                ticket.StaffId = editedTicket.Ticket.StaffId;

                await _corsacContext.SaveChangesAsync();

                if (editedTicket.StaffEdit && editedTicket.Comment != null && editedTicket.Comment.Trim().Length > 0)
                {
                    Customer customer = await _corsacContext.Customers.FindAsync(editedTicket.Ticket.CustomerId);
                    SendTicketEditedEmail(editedTicket.Ticket.ReferenceId, customer, editedTicket.Comment);
                }

                return RedirectToAction("Edit", new { refTicketId = editedTicket.Ticket.ReferenceId });
            }
            return View(editedTicket);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _corsacContext.Dispose();
            }
            base.Dispose(disposing);
        }

        protected string GenerateUniqueReferenceId()
        {
            string referenceId = _utilities.GenerateReferenceId();

            while (_corsacContext.Tickets.Any(t => t.ReferenceId == referenceId))
            {
                referenceId = _utilities.GenerateReferenceId();
            }

            return referenceId;
        }

        protected void SendTicketCreatedEmail(string ticketRef, Customer customer)
        {
            string emailSubject = $"Ticket Created:{ticketRef}";

            string emailBody = $@"Hi {customer.Name}!
                
                   Thanks for reaching out to us!  We have received your ticket and are investigating. 
                   Please expect a response from us within 24 hours.

                   Your ticket reference number is {ticketRef}. You can view your ticket and add comments at 
                    https://localhost:44343/Tickets/Edit?refTicketId={ticketRef}
                    
                    Kind Regards,
                    Corsac Support Team";

            _utilities.SendCustomerEmail(customer.EmailAddress, emailBody, emailSubject);
        }

        protected void SendTicketEditedEmail(string ticketRef, Customer customer, string ticketComment)
        {
            string emailSubject = $"Ticket Updated:{ticketRef}";

            string emailBody = $@"Hi {customer.Name}!
                
                   There's been a comment made on your ticket: 
                   
                   {ticketComment}

                   You can view your ticket and respond at 
                    https://localhost:44343/Tickets/Edit?refTicketId={ticketRef}
                    
                    Kind Regards,
                    Corsac Support Team";

            _utilities.SendCustomerEmail(customer.EmailAddress, emailBody, emailSubject);
        }
    }
}
