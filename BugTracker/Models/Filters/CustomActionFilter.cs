﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BugTracker.Models.Filters
{
    public class CustomActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var db = new ApplicationDbContext();

            var userId = filterContext.HttpContext.User.Identity.GetUserId();

            int id = Convert.ToInt32(filterContext.ActionParameters["id"]);
            
            if(id == 0)
            {
                filterContext.Result = new RedirectToRouteResult(
                 new RouteValueDictionary
                 {
                  { "controller", "Ticket" },
                    { "action", "Index" }
                 });
            }

            var ticket = db.Tickets.FirstOrDefault(p => p.Id == id);

            if (!filterContext.HttpContext.User.IsInRole("Admin") ||
                !filterContext.HttpContext.User.IsInRole("Project Manager"))
            {


                if (filterContext.HttpContext.User.IsInRole("Developer") && ticket.AssignedToUserId != userId)
                {
                    filterContext.Result = new RedirectToRouteResult(
                  new RouteValueDictionary
                  {
                  { "controller", "Ticket" },
                    { "action", "UnAthorizedAccess" }
                  });

                }

                else if (filterContext.HttpContext.User.IsInRole("Submitter") && ticket.OwnerUserId != userId)
                {
                    filterContext.Result = new RedirectToRouteResult(
                  new RouteValueDictionary
                  {
                  { "controller", "Ticket" },
                    { "action", "UnAthorizedAccess" }
                  });
                }
            }


        }

    }
}