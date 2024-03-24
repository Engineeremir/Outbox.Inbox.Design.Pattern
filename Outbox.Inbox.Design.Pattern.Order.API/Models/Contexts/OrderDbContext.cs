using Microsoft.EntityFrameworkCore;
using Outbox.Inbox.Design.Pattern.Order.API.Models.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Outbox.Inbox.Design.Pattern.Order.API.Models.Contexts
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Entities.Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderOutbox> OrderOutboxes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
