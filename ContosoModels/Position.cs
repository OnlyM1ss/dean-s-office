using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.Models
{
    public class Position : DbObject
    {
        public string Name { get; set; }
        public int Rating { get; set; }
    }
}
