using AuthBuisnessLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthBuisnessLayer.Services.Interfaces
{
    public interface IAuthService
    {
       Task<string> GetToken(UserLoginModelBL userInfo);
    }
}
