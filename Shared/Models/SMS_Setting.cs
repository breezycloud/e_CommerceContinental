using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace e_CommerceContinental.Shared.Models;

public class SMS_Setting
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required(ErrorMessage = "Username is required")]
    public string? Username { get; set; }
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    [Required(ErrorMessage = "Sender Name is required")]
    [StringLength(10, ErrorMessage = "Must between 5 and 10 Characters",MinimumLength = 5)]
    public string? SenderName { get; set; }
    public string? Url { get; set; }
}
