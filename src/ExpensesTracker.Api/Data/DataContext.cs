using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesTracker.Api.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Expense> Expenses { get; set; }  
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // GUID auto-gen
            modelBuilder.Entity<Expense>()
                .Property(e => e.Id)
                .HasDefaultValueSql("newid()");

            // amount decimal column type
            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            // timestamp auto-gen
            modelBuilder.Entity<Expense>()
                .Property(e => e.TimeStamp)
                .HasDefaultValueSql("getdate()");
        }
    }
}
