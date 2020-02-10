using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalLondon.Business
{
    public interface IPremiumCalculator
    {
        List<Customer> CustomerPremiumCalculator(List<Customer> lstCustomer,string logPath);
    }
}
