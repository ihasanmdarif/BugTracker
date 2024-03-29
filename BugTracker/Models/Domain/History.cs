﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.Domain
{
    public class History
    {
        public int Id { get; set; }
        public string Property { get; set; }

        public string OldValueName { get; set; }

        public string NewValueName { get; set; }

        public int TicketId { get; set; }
        public virtual Ticket Ticket{get;set;}

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        
    }
}