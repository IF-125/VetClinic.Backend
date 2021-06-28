using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace VetClinic.DAL.Context
{
    class VetClinicContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }

        public DbSet<PhoneNumber> PhoneNumbers { get; set; }

        public VetClinicContext(DbContextOptions<VetClinicContext> options) : base(options) { }
    }
}
