using System;
using System.Collections.Generic;
using System.Text;

namespace AuthMicroservice.BLL.Models.DTO.Token
{
    public class TokenDTO
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public int ExpiresIn { get; set; }
    }
}
