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






        // DbSets for the models
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        //public DbSet<Status> Statuses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        //public DbSet<Order> Orders { get; set; }
        //public DbSet<OrderItem> OrderItems { get; set; }
        //public DbSet<Payment> Payments { get; set; }
        //public DbSet<Cancellation> Cancellations { get; set; }
        //public DbSet<Refund> Refunds { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        //public DbSet<Feedback> Feedbacks { get; set; }
    }
}
