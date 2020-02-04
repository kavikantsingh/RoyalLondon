using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalLondon.Business
{
   public interface IFileGenerator
    {
        bool GenerateCustomerRenewalLetter(string filepath, List<Customer> lstCustomer);
    }
}
