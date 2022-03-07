using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helperland.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Helperland.Data
{
    public class AppDbContext : IdentityDbContext <AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public virtual DbSet<UserAddress> addresses { get; set; }
        public virtual DbSet<ServiceRequest> ServicesRequests { get; set; }
        public virtual DbSet<ServiceRequestAddress> ServiceRequestAddress { get; set; }
        public virtual DbSet<ServiceRequestExtra> ServiceRequestExtras { get; set; }
    }
}
