using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.Models
{
    public class Teacher : DbObject, IEquatable<Teacher>
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

        public bool Equals(Teacher other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return FirstName == other.FirstName && LastName == other.LastName && DateOfHiring.Equals(other.DateOfHiring);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Teacher) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (FirstName != null ? FirstName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (LastName != null ? LastName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ DateOfHiring.GetHashCode();
                return hashCode;
            }
        }
    }
}
