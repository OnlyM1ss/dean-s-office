using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.Models
{
    public class Teacher
    {
        /// <summary>
        /// имя
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// фамилия
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// дата найма
        /// </summary>
        public DateTime DateOfHiring { get; set; }
    }
}
