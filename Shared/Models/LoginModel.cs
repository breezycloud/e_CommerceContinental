using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace e_CommerceContinental.Shared.Models;
public class LoginModel
    {
        public Guid Id { get; set; }
        public Guid BranchID { get; set; }
        public Guid? ShopID { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 8 and 255 characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Token { get; set; }
        public string AccessPrivilege { get; set; }
        public bool Status { get; set; }
    }
