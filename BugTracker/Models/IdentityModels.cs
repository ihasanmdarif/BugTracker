﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using BugTracker.Models.AppHelper;
using BugTracker.Models.Domain;
using EntityFramework.DynamicFilters;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string ScreenName { get; set; }
        public virtual List<Project> Projects { get; set; }

        [InverseProperty("OwnerUser")]
        public virtual List<Ticket> Tickets { get; set; }

        public virtual List<Ticket> AssignedTickets { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<Attachment> Attachments { get; set; }

        public virtual List<History> Histories { get; set; }

        public virtual List<string> AssignRoles { get; set; }

        [InverseProperty("NotifyUsers")]
        public virtual List<Ticket> NotifyTickets { get; set; }


        public ApplicationUser()
        {
            Projects = new List<Project>();
            Tickets = new List<Ticket>();
            AssignedTickets = new List<Ticket>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("ScreenName", ScreenName.ToString()));

            return userIdentity;
        }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<TicketPriority> TicketPriorites { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<History> Histories { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany<Ticket>(p => p.NotifyTickets)
                .WithMany(p => p.NotifyUsers)
                .Map(x =>
                {
                    x.MapLeftKey("UserId");
                    x.MapRightKey("TicketId");
                    x.ToTable("TicketNotifications");
                });

            //custom filter to not include project that is archived and
            //not inclued ticket that has archived project

            modelBuilder.Filter("IsPArchived", (Project p) => !p.IsArchived);

            modelBuilder.Filter("IsTArchived", (Ticket t) => (!t.Project.IsArchived));
        }
    }
}