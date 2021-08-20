using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PyroDB.Models;

namespace PyroDB.Data
{
    public class PyroDBContext : DbContext
    {
        public PyroDBContext(DbContextOptions<PyroDBContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();

        }

        public DbSet<PyroDB.Models.Chemical> Chemical { get; set; }
        public DbSet<PyroDB.Models.User> User { get; set; }
        public DbSet<PyroDB.Models.Label> Label { get; set; }
    }
}
