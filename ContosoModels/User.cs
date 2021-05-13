using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.Models
{
    public class User :DbObject
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
