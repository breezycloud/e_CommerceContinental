using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using e_CommerceContinental.Shared.Models;

namespace e_CommerceContinental.Client.Services;

public interface IUserService
{
    public Task<LoginModel> Login(LoginModel model);
}
