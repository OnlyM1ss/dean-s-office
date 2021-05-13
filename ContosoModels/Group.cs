using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.Models
{
    public class Group : DbObject, IEquatable<Group>
    {
        /// <summary>
        /// Факультет
        /// </summary>
        public string Faculty { get; set; }
        /// <summary>
        /// Численность
        /// </summary>
        public int Count { get; set; }

        public bool Equals(Group other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Faculty == other.Faculty && Count == other.Count;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Group) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Faculty != null ? Faculty.GetHashCode() : 0) * 397) ^ Count;
            }
        }
    }
}
