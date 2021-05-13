using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.Models
{
    public class Discipline : DbObject
    {
        public int Name { get; set; }
        public int AcademyHours { get; set; }
    }
}
