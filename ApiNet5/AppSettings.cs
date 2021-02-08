using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiNet5
{
    public class Jwt
    {
        public string PrivateKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }


    public class AppSettings
    {
        public Jwt Jwt { get; set; }
        public string AllowedHosts { get; set; }
    }
}
