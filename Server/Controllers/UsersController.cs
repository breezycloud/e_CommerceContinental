using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using e_CommerceContinental.Server.Data;
using e_CommerceContinental.Shared.Models;
using e_CommerceContinental.Server.Util;

namespace HyLook.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IConfiguration Configuration {get;}
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginModel>> Login(LoginModel model)
        {
            string hashedPassword = Security.Encrypt(model.Password);
            var user = await _context.EmployeeAccounts.AsSplitQuery().Where(u => u.Username == model.Username 
                                        && u.HashedPassword == hashedPassword
                                        && u.IsActive == true)
                                       .Include(x => x.Employee) 
                                       .Include(r => r.Role)
                                       .FirstOrDefaultAsync();

            if (user is null)
            {
                return BadRequest();
            }

            var claim = new Claim[]
            {                
                new Claim(ClaimTypes.NameIdentifier, user.EmpID.ToString()),
                new Claim(ClaimTypes.Name, user.Username ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role!.RoleType ?? string.Empty)
            };

            var token = new JwtSecurityToken(
                null,
                null,
                claim,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["App:Key"])),
                SecurityAlgorithms.HmacSha512Signature));

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            var result = new LoginModel
            {
                Id = user.EmpID,
                Token = jwt,
                BranchID = user.Employee!.BranchID,
                ShopID = user.Employee.EmployeeAccount!.ShopID,
                AccessPrivilege = user.Role!.RoleType
            };                                                 
            // var identity = new ClaimsIdentity(claim);
            // HttpContext.User = new ClaimsPrincipal(identity);

            return result;
        }        
    }
}
