using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using VetClinic.Core.Entities;

namespace VetClinic.DAL.Context
{
    public class VetClinicContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }

        public DbSet<PhoneNumber> PhoneNumbers { get; set; }

        public VetClinicContext(DbContextOptions<VetClinicContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
