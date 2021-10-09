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

namespace Corsac_Ticketing_System.Controllers
{
    public class TicketsController : Controller
    {
        private readonly CorsacTicketContext _corsacContext; 

        public TicketsController()
        {
            _corsacContext = new CorsacTicketContext();
        }
        // GET: Tickets
        public async Task<ActionResult> Index()
        {
            return View(await _corsacContext.Tickets.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = await _corsacContext.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ReferenceId,Status,StaffId,CustomerId,SubjectLine,IssueDescription")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _corsacContext.Tickets.Add(ticket);
                await _corsacContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = await _corsacContext.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ReferenceId,Status,StaffId,CustomerId,SubjectLine,IssueDescription")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _corsacContext.Entry(ticket).State = EntityState.Modified;
                await _corsacContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(ticket);
        }

       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _corsacContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
