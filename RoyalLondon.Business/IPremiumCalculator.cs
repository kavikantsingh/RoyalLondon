using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalLondon.Business
{
    /// <summary>
    /// IPremium interface
    /// </summary>
    public interface IPremiumCalculator
    {
        /// <summary>
        /// This interface implemented to calculate premiums
        /// </summary>
        /// <param name="lstCustomer"></param>
        /// <param name="logPath"></param>
        /// <returns></returns>
        List<Customer> CustomerPremiumCalculator(List<Customer> lstCustomer, string logPath);
    }
}
