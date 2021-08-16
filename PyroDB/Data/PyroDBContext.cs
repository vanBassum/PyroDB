﻿using System;
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


        public DbSet<PyroDB.Models.Composition> Composition { get; set; }


        public DbSet<PyroDB.Models.Chemical> Chemical { get; set; }
    }
}
