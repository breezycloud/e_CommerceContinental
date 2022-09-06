using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_CommerceContinental.Shared.Models;

public class Employee
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();       
    [NotMapped]
    public string? FullName => $"{FirstName} {LastName}";
    [Required(ErrorMessage = "First Name is required")]
    public string? FirstName { get; set; }
    [Required(ErrorMessage = "Last Name is required")]
    public string? LastName { get; set; }
    public int Bday { get; set; }    
    public int Bmonth { get; set; }        
    [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Invalid email format.")]
    public string? Email { get; set; }
    [Required]
    [StringLength(11, ErrorMessage = "Phone No should be 11 characters", MinimumLength = 11)]
    public string? PhoneNo { get; set; }    
    public string? HomeAddress { get; set; }
    public Guid BranchID { get; set; }
    [ForeignKey(nameof(BranchID))]
    public virtual Branch? Branch { get; set; }    
    [Column(TypeName = "decimal(18, 2)")]
    [Required(ErrorMessage = "Salary is required")]
    public decimal? Salary { get; set; }    
    public Guid BankID { get; set; }
    [ForeignKey(nameof(BankID))]
    public virtual Bank? Bank { get; set; } 
    public string? AccountName { get; set; }
    [Required]
    [StringLength(10, ErrorMessage = "Account No should be 11 characters", MinimumLength = 10)]
    public string? AccountNo { get; set; }    
    public string? ImageBase64 { get; set; }
    public DateTime? EmpLoyedDate { get; set; }
    public DateTime? ExitDate { get; set; }
    public DateTime? ModifiedDate { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;
    public virtual EmployeeAccount? EmployeeAccount { get; set; } =new();
    // public virtual List<Salary> Salaries { get; set; } = new();
    // public virtual List<SalaryAdvance> SalaryAdvances { get; set; } = new();
    // public virtual List<SalaryBonus> SalaryBonus { get; set; } = new();
    // public virtual List<SalaryPenalty> SalaryPenalties { get; set; } = new();
    // public virtual List<NotificationMessage> NotificationMessages { get; set; } = new();
}