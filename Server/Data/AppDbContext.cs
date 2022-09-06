
using e_CommerceContinental.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace e_CommerceContinental.Server.Data;

#nullable disable

public partial class AppDbContext :  DbContext
{        

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Stock> Stock { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<OrderDetails> OrderDetails { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Shop> Shops { get; set; }
    public DbSet<Bank> Banks { get; set; }    
    public DbSet<Customer> Customers { get; set; }    
    public DbSet<Message> Messages { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Role> Roles { get; set; }    
    public DbSet<SMS_Setting> SMS_Settings { get; set; }    
    public DbSet<Department> Departments { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeAccount> EmployeeAccounts { get; set; }        
    public DbSet<OrderPayment> OrderPayments { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<UserMenus> UserMenus { get; set; }    
    public DbSet<SubMenu> SubMenus { get; set; }
    public DbSet<UserSubMenus> UserSubMenus { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
      optionsBuilder.EnableSensitiveDataLogging();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {        
                
        modelBuilder.Entity<OrderDetails>()
                    .HasOne(x => x.Order)
                    .WithMany(x => x.OrderDetails)
                    .OnDelete(DeleteBehavior.Restrict);
                  
        modelBuilder.Entity<Message>()
                    .Property(x => x.Id)
                    .UseIdentityColumn(1, 1);        

        modelBuilder.Entity<Menu>()
                    .Property(x => x.Order)
                    .UseIdentityColumn(1, 1);        
        
        modelBuilder.Entity<SubMenu>()
                    .Property(x => x.Order)
                    .UseIdentityColumn(1, 1);        
    }                
}