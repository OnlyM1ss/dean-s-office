using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Contoso.Models
{
    public class Discipline : DbObject, IEquatable<Discipline>
    {
        public string Name { get; set; }
        public int AcademyHours { get; set; }

        public bool Equals(Discipline other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && AcademyHours == other.AcademyHours;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Discipline) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ AcademyHours;
            }
        }
    }
}
