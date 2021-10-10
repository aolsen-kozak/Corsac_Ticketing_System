using Corsac_Ticketing_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Corsac_Ticketing_System.ViewModels
{
    public class CreateTicket
    {
        public string CustomerName { get; set; }
        public string CustomerEmailAddress { get; set; }
        public Departments Department { get; set; }
        public string SubjectLine { get; set; }
        public string IssueDescription { get; set; }
    }
}