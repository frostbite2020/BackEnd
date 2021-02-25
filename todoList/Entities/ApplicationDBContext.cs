using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Context
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get;set; }

        public DbSet<TodoCategory> Categories { get; set; }
        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Account> Accounts { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<TodoCategory>().ToTable("TodoCategory");
            builder.Entity<TodoList>().ToTable("TodoList");
        }
    }
}
