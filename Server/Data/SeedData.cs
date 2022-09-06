using System.Globalization;
using e_CommerceContinental.Server.Data;
using e_CommerceContinental.Shared.Models;

namespace e_CommerceContinental.Server.Data;

public static class SeedData
{    
    public static void EnsureSeeded(IServiceProvider services)
    {        
        var scopeFactory = services.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();               
        //var dir = @"C:\Users\Aminu\Documents\CSV Data\";                        
        try
        {
            //db.Database.EnsureDeleted();
            if (db.Database.EnsureCreated())
            {
                AddBranch(db);
                AddShops(db);
                // AddDepartment(db);
                AddRoles("", db);            
                GetBanks("", db);                
                AddEmployee(db);
                AddCustomers(db);
                // AddAccounts(db);                
                AddMessages("", db);                
                AddCategories(db);
                AddProducts(db);
            }                   
        }
        catch (System.Exception)
        {            
            throw;
        }        
    }

    private static void AddShops(AppDbContext db)
    {
        db.Shops.AddRange(new Shop[]
        {
            new Shop
            {
                Id = Guid.NewGuid(),
                ShopName = "Computer Accessories",
                BranchID = db.Branches.FirstOrDefault()!.Id,
                ModifiedDate = DateTime.Now
            },
            new Shop
            {
                Id = Guid.NewGuid(),
                ShopName = "Electronics Accessories",
                BranchID = db.Branches.FirstOrDefault()!.Id,
                ModifiedDate = DateTime.Now
            }
        });
        db.SaveChanges();
    }

    private static void AddBranch(AppDbContext db)
    {
        db.Branches.AddRange(new Branch[]
        {
            new Branch
            {
                Id = Guid.NewGuid(),
                BranchName = "Company",
                BranchAddress ="Abuja", 
                PhoneNo1 = "08030203023"               
            }
        });
        db.SaveChanges();
    }

    private static void AddCategories(AppDbContext db)
    {
        Guid sID = Guid.NewGuid();
        Guid bID = Guid.NewGuid();
        Guid fID = Guid.NewGuid();
        Guid wbID = Guid.NewGuid();
        db.Category.AddRange(new Category[]
        {            
            new Category
            {
                Id = sID,
                CategoryName = "Laptops",
                Products = new List<Product>
                {
                    new Product
                    {
                        Id = bID,
                        Code = "09020",
                        ProductName = "HP Pavillion 15",
                        UnitPrice = 350000,
                        StocksOnHand = 5,
                        CategoryID = sID,
                        ShopID = db.Shops.Where(x => x.ShopName == "Computer Accessories").FirstOrDefault()!.Id,                        
                        Stocks = new List<Stock>()
                        {
                            new Stock
                            {
                                Id = Guid.NewGuid(),
                                ProductID = bID,
                                Quantity = 5,
                                Date = DateTime.Now,
                                ModifiedDate =  DateTime.Now,

                            }
                        }
                    }                    
                }
            },
            new Category
            {
                Id = Guid.NewGuid(),
                CategoryName = "TVs"                
            },
            new Category
            {
                Id = Guid.NewGuid(),
                CategoryName = "Phones"
            }
        });
        db.SaveChanges();
    }

    private static void AddProducts(AppDbContext db)
    {
        List<Product> products = new();
        Guid ShopID = db.Shops.Select(x => x.Id).FirstOrDefault();
        foreach (var item in GetCategoriesAsync(db))
        {
            for (int i = 0; i < 10; i++)
            {
                Guid ProductID = Guid.NewGuid();
                products.Add(new Product
                {
                    Id = ProductID,
                    Code = $"{item.ToString().Substring(0, 3).First()}",
                    ProductName = $"{item.ToString().Substring(0, 4).First()}{i}",
                    StocksOnHand = 10,
                    ReorderLevel = 1,
                    CategoryID = item,
                    UnitPrice = (1000 * 10) * (i + 1),
                    ShopID = ShopID,
                    Stocks = new List<Stock>
                    {
                        new Stock
                        {
                            Id = Guid.NewGuid(),
                            ProductID = ProductID,
                            Quantity = 10,
                            Date = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                        }
                    }                    
                });
            }
        }
        db.Products.AddRange(products);
        db.SaveChanges();        
    }        
    private static List<Guid> GetCategoriesAsync(AppDbContext db)
    {
        return db.Category.Select(x => x.Id).ToList();
    }
    private static void AddAccounts(AppDbContext db)
    {
        db.Accounts.AddRange(new Account[]
        {
            new Account
            {
                Id = Guid.NewGuid(),
                Code = 101001,
                AccountName = "Tailoring Materials"                
            },
            new Account
            {
                Id = Guid.NewGuid(),
                Code = 101002,
                AccountName = "Logistics"                
            }
        });
        db.SaveChanges();
    }
    
    private static void AddEmployee(AppDbContext db)
    {
        Guid master = Guid.NewGuid();
        Guid sales = Guid.NewGuid();
        Guid store = Guid.NewGuid();
        db.Employees.Add(new Employee
        {            
            Id = master,
            FirstName = "Aminu",
            LastName = "Aliyu Muhammad",
            Bday = 11,
            Bmonth = 12,
            Email = "aminuali13@gmail.com",
            PhoneNo = "07068716813",
            HomeAddress = "Wuse II, Abuja",            
            Salary = 300000,
            BankID = db.Banks.Where(x => x.BankCode == "580").Select(x => x.Id).FirstOrDefault(),
            BranchID = db.Branches.FirstOrDefault()!.Id,
            AccountNo = "0000000000",
            AccountName = "Aliyu Aminu",
            EmpLoyedDate = DateTime.Now,
            EmployeeAccount =  new EmployeeAccount
            {            
                Id = Guid.NewGuid(),
                EmpID = master,
                Username = "nerdyamin",
                HashedPassword = "5785a75e6fd861d4b59795625d74875d04cbbac32c0c0a8f154a9c29cf83414ca4d559db59032c70855180003b8f57b79102d3187fe30f3e036b0bd7b7acde2d",
                IsActive = true,
                RoleId = db.Roles.Where(x => x.RoleType == "Master").Select(x => x.RoleId).FirstOrDefault(),                            
            }
        });
        db.Employees.Add(new Employee
        {            
            Id =sales,
            FirstName = "Sale",
            LastName = "Manager",
            Bday = 11,
            Bmonth = 12,
            Email = "sales@gmail.com",
            PhoneNo = "07068716812",
            HomeAddress = "Wuse II, Abuja",            
            Salary = 300000,
            BankID = db.Banks.Where(x => x.BankCode == "580").Select(x => x.Id).FirstOrDefault(),
            BranchID = db.Branches.FirstOrDefault()!.Id,
            AccountNo = "0000000000",
            AccountName = "Manager Sales",
            EmpLoyedDate = DateTime.Now,
            EmployeeAccount = new EmployeeAccount
            {            
                Id = Guid.NewGuid(),
                EmpID = sales,
                Username = "sales",
                HashedPassword = "5785a75e6fd861d4b59795625d74875d04cbbac32c0c0a8f154a9c29cf83414ca4d559db59032c70855180003b8f57b79102d3187fe30f3e036b0bd7b7acde2d",
                IsActive = true,
                RoleId = db.Roles.Where(x => x.RoleType == "Sales").Select(x => x.RoleId).FirstOrDefault(),            
                ShopID = db.Shops.FirstOrDefault()!.Id            
            }
        });
        db.Employees.Add(new Employee
        {            
            Id = store,
            FirstName = "Store",
            LastName = "Manager",
            Bday = 11,
            Bmonth = 12,
            Email = "store@gmail.com",
            PhoneNo = "07068716890",
            HomeAddress = "Wuse II, Abuja",            
            Salary = 300000,
            BankID = db.Banks.Where(x => x.BankCode == "580").Select(x => x.Id).FirstOrDefault(),
            BranchID = db.Branches.FirstOrDefault()!.Id,
            AccountNo = "0000000000",
            AccountName = "Manager Store",
            EmpLoyedDate = DateTime.Now,
            EmployeeAccount = new EmployeeAccount
            {            
                Id = Guid.NewGuid(),
                EmpID = store,
                Username = "store",
                HashedPassword = "5785a75e6fd861d4b59795625d74875d04cbbac32c0c0a8f154a9c29cf83414ca4d559db59032c70855180003b8f57b79102d3187fe30f3e036b0bd7b7acde2d",
                IsActive = true,
                RoleId = db.Roles.Where(x => x.RoleType == "Store").Select(x => x.RoleId).FirstOrDefault(),            
                ShopID = db.Shops.FirstOrDefault()!.Id                
            }
        });
        db.SaveChanges();
    }

    public static void GetBanks(string path, AppDbContext db)  
    {
        db.Banks.AddRange(new Bank[]
        {
            new Bank
            {
                Id = Guid.NewGuid(),
                BankCode = "580",
                BankName = "GT Bank"
            }            
        });
        db.SaveChanges();
    }    
    public static void AddCustomers(AppDbContext db)  
    {
        var random = new Random();
        string[] names = new string[]{ "Muhammad", "Aminu", "Kabir", "Umar", "Salisu" };
        List<Customer> customers = new();
        for (int i = 0; i < 100; i++)
        {  
            customers.Add(new Customer
            {
                CustomerId = Guid.NewGuid(),
                CustomerName = $"{names[random.Next(1, 4)]} {names[random.Next(1,4)]}",
                PhoneNo = i <= 9 ? $"0706871681{i}" : $"070687197{i}",
                ContactAddress = "115 Ahmadu Bello Way",
                Bday = 12,
                Bmonth = 11                
            });

        }        
        db.Customers.AddRange(customers);
        db.SaveChanges();
    }
    

    public static void AddMessages(string path, AppDbContext db)  
    {        
        db.Messages.Add(new Message
        {
            Msg = "Dear Valued Customer, You have successfully checked in your order with receipt number: Thank You for choosing HYLOOK"    
        }); 
        db.SaveChanges();
        db.Messages.Add(
        new Message
        {
            Msg = "Dear Customer, Your Order with Receipt No: is ready for collection. We thank you for your continuous patronage and choosing HYLOOK" 
        }); 
        db.SaveChanges();
        db.Messages.Add(
        new Message
        {
            Msg = "Dear Customer, Your Order with Receipt No: is ready for fitting. We thank you for your continuous patronage and choosing HYLOOK"             
        }); 
        db.SaveChanges();
        db.Messages.Add(
        new Message
        {
            Msg = "Dear Customer, Thank you for purchasing our product. Your support and trust in us are much appreciated." 
        }); 
        db.SaveChanges();                         
    }

    public static void AddRoles(string path, AppDbContext db)  
    {
        db.Roles.AddRange(new Role[]
        {
            new Role
            {
                RoleId = Guid.NewGuid(),
                RoleType = "Master"
            },
            new Role
            {
                RoleId = Guid.NewGuid(),
                RoleType = "Admin"
            },
            new Role
            {
                RoleId = Guid.NewGuid(),
                RoleType = "Store"
            },
            new Role
            {
                RoleId = Guid.NewGuid(),
                RoleType = "Manager"
            },
            new Role
            {
                RoleId = Guid.NewGuid(),
                RoleType = "Sales"
            }       
        });        
        db.SaveChanges();
    }      

    private static void AddMenus(AppDbContext db)
    {
        var menus = new Menu[]
        {
            new Menu
            {
                Id = Guid.NewGuid(),
                Order = 1,
                MenuName = "Dashboard",
                ModifiedDate = DateTime.Now,
            },
            new Menu
            {
                Id = Guid.NewGuid(),
                Order = 2,
                MenuName = "Customers",
                ModifiedDate = DateTime.Now,
            },
            new Menu
            {
                Id = Guid.NewGuid(),
                Order = 3,
                MenuName = "Sales",
                ModifiedDate = DateTime.Now,
            },
            new Menu
            {
                Id = Guid.NewGuid(),
                Order = 4,
                MenuName = "Products",
                ModifiedDate = DateTime.Now,
            },
            new Menu
            {
                Id = Guid.NewGuid(),
                Order = 5,
                MenuName = "Users",
                ModifiedDate = DateTime.Now,
            }
        };
        foreach (var item in menus.OrderBy(x => x.Order))
            db.Menus.Add(new Menu { Id = item.Id, MenuName = item.MenuName, ModifiedDate = item.ModifiedDate });
        db.SaveChanges();
    }
}