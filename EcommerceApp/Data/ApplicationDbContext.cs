using EcommerceApp.Models;
using ECommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            // Product -> Category
        
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            
            // Order -> Customer
                                                                        
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order -> Addresses
        
            modelBuilder.Entity<Order>()
                .HasOne(o => o.BillingAddress)
                .WithMany()
                .HasForeignKey(o => o.BillingAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.ShippingAddress)
                .WithMany()
                .HasForeignKey(o => o.ShippingAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment -> Order
        
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

  
            // Cancellation -> Order
        
            modelBuilder.Entity<Cancellation>()
                .HasOne(c => c.Order)
                .WithOne(o => o.Cancellation)
                .HasForeignKey<Cancellation>(c => c.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Refund -> Payment & Cancellation
         
            // Refund -> Payment
            modelBuilder.Entity<Refund>()
                .HasOne(r => r.Payment)
                .WithOne(p => p.Refund)
                .HasForeignKey<Refund>(r => r.PaymentId)
                .OnDelete(DeleteBehavior.Restrict); // important: prevents multiple cascade paths

            // Refund -> Cancellation
            modelBuilder.Entity<Refund>()
                .HasOne(r => r.Cancellation)
                .WithOne(c => c.Refund)
                .HasForeignKey<Refund>(r => r.CancellationId)
                .OnDelete(DeleteBehavior.Restrict); // important: prevents multiple cascade paths

            
            // Feedback -> Customer & Product
          
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Customer)
                .WithMany(c => c.Feedbacks)
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Product)
                .WithMany(p => p.Feedbacks)
                .HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

         
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Electronic devices", IsActive = true },
                new Category { Id = 2, Name = "Books", Description = "Books and magazines", IsActive = true }
            );

            // Statuses (Order, Payment, Refund, Cancellation)
            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 1, Name = "Pending" },
                new Status { Id = 2, Name = "Processing" },
                new Status { Id = 3, Name = "Shipped" },
                new Status { Id = 4, Name = "Delivered" },
                new Status { Id = 5, Name = "Canceled" },
                new Status { Id = 6, Name = "Completed" },
                new Status { Id = 7, Name = "Failed" },
                new Status { Id = 8, Name = "Approved" },
                new Status { Id = 9, Name = "Rejected" },
                new Status { Id = 10, Name = "Refunded" }
            );
        }

        // DbSets
       
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Cancellation> Cancellations { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
    }
}
