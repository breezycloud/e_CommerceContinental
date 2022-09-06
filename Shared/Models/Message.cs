using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace e_CommerceContinental.Shared.Models;
public class Message
{
    [Key]
    public int Id { get; set; }
    public string? Msg { get; set; }
}
