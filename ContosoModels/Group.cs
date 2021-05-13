using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.Models
{
    public class Group : DbObject
    {
        /// <summary>
        /// Факультет
        /// </summary>
        public string Faculty { get; set; }
        /// <summary>
        /// Численность
        /// </summary>
        public int Count { get; set; }
    }
}
