using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalLondon.Business
{
    /// <summary>
    /// This interface implemented to generate CSV file
    /// </summary>
    public interface IFileGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="lstCustomer"></param>
        /// <param name="logPath"></param>
        /// <returns></returns>
        bool GenerateCustomerRenewalLetter(string filepath, List<Customer> lstCustomer, string logPath);
    }
}
