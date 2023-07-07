using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProManage
{
    public class Customer : IEquatable<Customer>
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public bool Equals(Customer other)
        {
            if (other == null)
                return false;

            return this.Name == other.Name;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}
