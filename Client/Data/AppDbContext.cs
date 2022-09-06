using e_CommerceContinental.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace e_CommerceContinental.Client.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) =>
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;    
    
    public DbSet<Stock> Stock { get; set; } = default!;
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<OrderDetails> OrderDetails { get; set; } = default!;
    public DbSet<Category> Category { get; set; } = default!;
    public DbSet<Branch> Branches { get; set; } = default!;
    public DbSet<Shop> Shops { get; set; } = default!;
    public DbSet<Bank> Banks { get; set; } = default!;    
    public DbSet<Customer> Customers { get; set; } = default!;    
    public DbSet<Message> Messages { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<Role> Roles { get; set; } = default!;    
    public DbSet<SMS_Setting> SMS_Settings { get; set; } = default!;    
    public DbSet<Department> Departments { get; set; } = default!;
    public DbSet<Account> Accounts { get; set; } = default!;
    public DbSet<Employee> Employees { get; set; } = default!;
    public DbSet<EmployeeAccount> EmployeeAccounts { get; set; } = default!;        
    public DbSet<OrderPayment> OrderPayments { get; set; } = default!;
    public DbSet<Menu> Menus { get; set; } = default!;
    public DbSet<SubMenu> SubMenus { get; set; } = default!;
    public DbSet<UserPermission> UserPermissions { get; set; } = default!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {        
                
        modelBuilder.Entity<OrderDetails>()
                    .HasOne(x => x.Product)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);           
        // modelBuilder.Entity<Message>()
        //             .Property(x => x.Id)
        //             .UseIdentityColumn(1, 1);        
    }
}